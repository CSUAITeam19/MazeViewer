using System;
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
        [SerializeField] private Camera thisCamera = null;
        [SerializeField] private FPController fpController = null;
        [SerializeField] private OrthoController orthoController = null;
        
        private void Awake()
        {
            if(thisCamera == null) thisCamera = GetComponent<Camera>();
            if(fpController == null) fpController = GetComponent<FPController>();
            if(orthoController == null) orthoController = GetComponent<OrthoController>();
        }

        /// <summary>
        /// 切换模式
        /// </summary>
        public void Switch()
        {
            if(fpController.enabled)
            {
                // 切换为正交
                thisCamera.orthographic = true;
                fpController.enabled = false;
                orthoController.enabled = true;
                // 考虑到可能下落到过低的位置, 重设一下y
                Vector3 posTemp = transform.position;
                transform.position = new Vector3(posTemp.x, 2.0f, posTemp.z);
                // 往下看
                thisCamera.transform.LookAt(transform.position - Vector3.up, -Vector3.right);
            }
            else
            {
                // 切换为透视
                thisCamera.orthographic = false;
                fpController.enabled = true;
                orthoController.enabled = false;
            }
        }
    }

}