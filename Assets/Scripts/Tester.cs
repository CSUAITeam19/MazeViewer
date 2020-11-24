using System;
using System.Threading;
using IPC;
using UnityEngine;

namespace MazeViewer
{
    /// <summary>
    /// Do some simple test here
    /// </summary>
    public class Tester : MonoBehaviour
    {
        private Thread thread;

        [ContextMenu("Kill Thread")]
        public void KillThread()
        {
            thread?.Abort();
        }
        [ContextMenu("Test")]
        public void Test()
        {
            Debug.Log("Test something");
            //string a = "123 123";
            //var callWatch = System.Diagnostics.Stopwatch.StartNew();
            //Thread thread = new Thread(() =>
            //{
            //    var watch = System.Diagnostics.Stopwatch.StartNew();
            //    for (int i = 0; i < 1e5; i++)
            //    {
            //        var l = new List<string>(a.Split(' '));
            //    }
            //    Debug.Log($"Time used:{watch.ElapsedMilliseconds}ms.");
            //});
            //thread.Start();
            //thread?.Abort();
            //thread = new Thread(() =>
            //{
            //    try
            //    {
            //        using(MazeEditorProxy proxy = new MazeEditorProxy())
            //        {
            //            proxy.SendFrameString("Hey! this msg is from unity!");
            //            string msg = proxy.ReceiveFrameString();
            //            UnityEngine.Debug.Log("Msg received is: \"" + msg + "\"");
            //        }
            //    }
            //    catch(Exception e)
            //    {
            //        UnityEngine.Debug.LogError("error: " + e);
            //        throw;
            //    }
            //});
            //thread.Start();
            
        }
    } 
}
