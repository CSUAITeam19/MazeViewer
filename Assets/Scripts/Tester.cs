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
        private void Update()
        {
            Camera main = Camera.main;
            var pos = Input.mousePosition;
            var ray = main.ScreenPointToRay(pos);
            Debug.DrawLine(main.transform.position, main.transform.position + ray.direction * 100f);
            var hits = Physics.RaycastAll(ray);
            string logStr = "";
            foreach (var raycastHit in hits)
            {
                logStr += $"{raycastHit.transform.name}, ";
            }
            Debug.Log("Hits: " + logStr);
        }
    } 
}
