using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using MazeViewer.Viewer;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class MazeEditorProxy : MonoBehaviour
{
    public enum MazeEditorEventType
    {
        /// <summary>
        /// 握手
        /// </summary>
        HandShake,
        /// <summary>
        /// 更新迷宫
        /// </summary>
        Update,
        /// <summary>
        /// 迷宫文件路径更新
        /// </summary>
        MazePathChange,
        /// <summary>
        /// 结果路径更新
        /// </summary>
        ResultPathChange,
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknown
    }
    public struct MazeEditorEvent
    {
        public MazeEditorEventType eventType;
        public string data;
        public MazeEditorEvent(MazeEditorEventType eventType, string data)
        {
            this.eventType = eventType;
            this.data = data;
        }
    }
    private RequestSocket socket;
    private Thread sockeThread;
    private bool terminated = false;

    private ConcurrentQueue<MazeEditorEvent> eventQueue = new ConcurrentQueue<MazeEditorEvent>();

    public static MazeEditorProxy instance
    {
        get;
        private set;
    }

    public event Action<MazeEditorEvent> editorHandShakeEvent;
    public event Action<MazeEditorEvent> mazeUpdateEvent;
    public event Action<MazeEditorEvent> mazePathChangeEvent;
    public event Action<MazeEditorEvent> resultPathChangeEvent;

    private void Start()
    {
        // 建立单例
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Cannot have duplicated MazeEditorProxy in one scene!");
            return;
        }
        
        sockeThread = new Thread(SocketLoop);
        sockeThread.Start();

    }

    private void Update()
    {
        while(eventQueue.TryDequeue(out var editorEvent))
        {
            switch(editorEvent.eventType)
            {
                case MazeEditorEventType.HandShake:
                    editorHandShakeEvent?.Invoke(editorEvent);
                    break;
                case MazeEditorEventType.Update:
                    mazeUpdateEvent?.Invoke(editorEvent);
                    break;
                case MazeEditorEventType.MazePathChange:
                    mazePathChangeEvent?.Invoke(editorEvent);
                    break;
                case MazeEditorEventType.ResultPathChange:
                    resultPathChangeEvent?.Invoke(editorEvent);
                    break;
                case MazeEditorEventType.Unknown:
                    Debug.LogError("Unknown msg!");
                    break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        ClearSocket();
    }

    /// <summary>
    /// 解析接收数据
    /// </summary>
    private void FrameStringProcess(string toParse)
    {
        string head;
        string data;
        int indexOfData = toParse.IndexOf(' ') + 1;
        if(indexOfData > 0)
        {
            head = toParse.Substring(0, indexOfData - 1);
            data = toParse.Substring(indexOfData);
        }
        else
        {
            head = toParse;
            data = "";
        }

        MazeEditorEventType eventType;
        switch(head)
        {
            // TODO: change header standard to unify name style
            case "Maze_Server":
                eventType = MazeEditorEventType.HandShake;
                break;
            case "Request_update":
                eventType = MazeEditorEventType.Update;
                break;
            case "path_maze":
                eventType = MazeEditorEventType.MazePathChange;
                break;
            case "answer":
                eventType = MazeEditorEventType.ResultPathChange;
                break;
            default:
                Debug.LogError($"Unknown head! head: {head}, data: {data}");
                eventType = MazeEditorEventType.Unknown;
                break;
        }

        eventQueue.Enqueue(new MazeEditorEvent(eventType, data));
    }

    /// <summary>
    /// 套接字循环, 在背景线程中运行
    /// </summary>
    private void SocketLoop()
    {
#if UNITY_EDITOR
        // use this to avoid freeze when reload script with (a) scenes running
        UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += ClearSocket;
#endif
        // 初始化AsyncIO
        AsyncIO.ForceDotNet.Force();
        terminated = false;
        using(socket = new RequestSocket(ConfigManager.instance.Current.mazeEditorAddress))
        {
            try
            {
                socket.SendFrame("Unity_Client");
                FrameStringProcess(socket.ReceiveFrameString());
                while (!terminated)
                {
                    socket.SendFrameEmpty();
                    FrameStringProcess(socket.ReceiveFrameString());
                }
            }
            catch (TerminatingException)
            {
                Debug.Log("Socket terminated.");
            }
            finally
            {
                socket.Dispose();
                NetMQConfig.Cleanup();
            }
        }
    }

    /// <summary>
    /// 清除所有由于使用socket产生的资源
    /// </summary>
    private void ClearSocket()
    {
        NetMQConfig.Cleanup();
        terminated = true;
#if UNITY_EDITOR
        // It works!
        UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= ClearSocket;
#endif
    }
}
