using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MazeViewer
{
    /// <summary>
    /// Do some simple test here
    /// </summary>
    public class Tester : MonoBehaviour
    {
        [ContextMenu("Test")]
        public void Test()
        {
            string a = "123 123";
            var callWatch = System.Diagnostics.Stopwatch.StartNew();
            Thread thread = new Thread(() =>
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                for (int i = 0; i < 1e5; i++)
                {
                    var l = new List<string>(a.Split(' '));
                }
                Debug.Log($"Time used:{watch.ElapsedMilliseconds}ms.");
            });
            thread.Start();
        }
    } 
}
