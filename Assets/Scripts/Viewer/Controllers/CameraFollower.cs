using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeViewer.Viewer.Control
{
    /// <summary>
    /// 摄像机跟随器
    /// </summary>
    public class CameraFollower : MonoBehaviour
    {
        public Transform toFollow;
        public Vector3 offset;

        private void Update()
        {
            transform.position = toFollow.position + offset;
        }
    } 
}
