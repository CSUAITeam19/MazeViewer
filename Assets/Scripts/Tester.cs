using System;
using System.Threading;
using System.Threading.Tasks;
using IPC;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

namespace MazeViewer
{
    /// <summary>
    /// Do some simple test here
    /// </summary>
    public class Tester : MonoBehaviour
    {
        private void Start()
        {
            MazeEditorProxy.instance.editorHandShakeEvent += e => { Debug.Log("Hand shake!"); };
            MazeEditorProxy.instance.mazePathChangeEvent += e => { Debug.Log($"Maze path changed! path: {e.data}"); };
        }

        //[ContextMenu("Close Socket")]
        //public void CloseSocket()
        //{
        //    socket?.Close();
        //    // call this to clean up everything
        //    NetMQConfig.Cleanup();
        //    task.Wait();
        //}

        //[ContextMenu("Test")]
        //public void Test()
        //{
        //    thread = new Thread(RunSocket);
        //    thread.IsBackground = true;
        //    thread.Start();
        //    UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += Stop;
        //    // UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += () => { NetMQConfig.Cleanup(); };
        //}

        //private void RunSocket()
        //{
        //    try
        //    {
        //        AsyncIO.ForceDotNet.Force();
        //        using (var socket = new RequestSocket(address))
        //        {
        //            Debug.Log("address: " + address);
        //            socket.SendFrame("Hello!");
        //            Debug.Log("SendFrame called.");
        //            string msg;
        //            bool success = socket.TryReceiveFrameString(TimeSpan.FromSeconds(10), out msg);
        //            Debug.Log($"ReceiveFrame called. success: {success}, msg: {msg}");
        //        }
        //        NetMQConfig.Cleanup();
        //        UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= Stop;
        //        // fucking compatibly of Unity and NetMQ , but fixed now!
        //    }
        //    catch(TerminatingException)
        //    {
        //        Debug.Log("socket terminated.");
        //    }
        //}

        //private void OnApplicationQuit()
        //{
        //    Debug.Log("On Application Quit");
        //}

        //private void OnDisable()
        //{
        //    Debug.Log("On Disable");
        //}

        //[ContextMenu("Stop")]
        //public void Stop()
        //{
        //    NetMQConfig.Cleanup(false);
        //    Debug.Log("Clean up.");
        //    UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= Stop;
        //    Debug.Log("Cleaned reloadEvent");
        //}
    } 
}
