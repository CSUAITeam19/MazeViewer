﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeViewer.Viewer.Control
{
    /// <summary>
    /// 切换投影模式
    /// </summary>
    public class ProjectionModeSwitch : MonoBehaviour
    {
        private Camera camera;
        private FPController fpController;
        private OrthoController orthoController;
        
        private void Awake()
        {
            camera = GetComponent<Camera>();
            fpController = GetComponent<FPController>();
            orthoController = GetComponent<OrthoController>();
        }

        /// <summary>
        /// 切换模式
        /// </summary>
        public void Switch()
        {
            if(fpController.enabled)
            {
                // 切换为正交
                camera.orthographic = true;
                fpController.enabled = false;
                orthoController.enabled = true;
                // 考虑到可能下落到过低的位置, 重设一下y
                Vector3 posTemp = transform.position;
                if (posTemp.y < 0) transform.position = new Vector3(posTemp.x, 5.0f, posTemp.z);
                // 往下看
                transform.LookAt(transform.position - Vector3.up, Vector3.forward);
            }
            else
            {
                // 切换为透视
                camera.orthographic = false;
                fpController.enabled = true;
                orthoController.enabled = false;
            }
        }
    }

}