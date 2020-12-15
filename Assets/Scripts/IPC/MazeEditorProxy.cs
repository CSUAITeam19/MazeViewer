using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using MazeViewer.UI;
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
    private NetMQSocket socket;
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
            }

            StatusInfo.Instance.PrintInfo($"获取类型为{editorEvent.eventType}的套接字信息:{editorEvent.data}");
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
            // TODO: allow result update separately from maze update
            case "Maze_Server":
                eventType = MazeEditorEventType.HandShake;
                break;
            case "update":
                eventType = MazeEditorEventType.Update;
                break;
            case "maze_path":
                eventType = MazeEditorEventType.MazePathChange;
                break;
            case "answer_path":
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
        using(socket = new PullSocket(ConfigManager.instance.Current.mazeEditorAddress))
        {
            try
            {
                // socket.SendFrame("Unity_Client");
                Debug.Log("Now wait for new msg.");
                FrameStringProcess(socket.ReceiveFrameString());
                Debug.Log("received.");
                while (!terminated)
                {
                    // socket.SendFrameEmpty();
                    Debug.Log("Now wait for new msg.");
                    FrameStringProcess(socket.ReceiveFrameString());
                    Debug.Log("received.");
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

    /// <summary>
    /// 重新加载套接字相关的事项
    /// </summary>
    [ContextMenu("Reset Everything")]
    public void ResetEverything()
    {
        NetMQConfig.Cleanup();
        terminated = true;
        sockeThread.Abort();
        sockeThread = new Thread(SocketLoop);
        sockeThread.Start();
    }
}
