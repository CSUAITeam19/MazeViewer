using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MazeViewer.Viewer.Control
{
    public class OrthoController : MonoBehaviour
    {
        public float moveSpeed;

        void Update()
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                // 如果没有选中某个物体
                // 只允许上下左右移动
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    x -= Input.GetAxis("Mouse X");
                    y -= Input.GetAxis("Mouse Y");
                }

                transform.position += moveSpeed * (x * transform.right + y * transform.up);
            }
        }
    }

}