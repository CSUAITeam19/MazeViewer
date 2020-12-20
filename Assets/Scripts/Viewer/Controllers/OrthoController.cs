using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace MazeViewer.Viewer.Control
{
    public class OrthoController : MonoBehaviour
    {
        public float moveSpeed = 0.2f;
        public float mouseScale = 5f;
        public float zoomRate = 1.0f;

        private Vector2 lastMousePos;
        [SerializeField] private Camera thisCamera = null;

        private void Awake()
        {
            if (thisCamera == null) thisCamera = GetComponent<Camera>();
        }

        void Update()
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                // 如果没有选中某个物体
                // 只允许上下左右移动
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                transform.position += moveSpeed * (x * transform.right + y * transform.up);

                if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    lastMousePos = Input.mousePosition;
                }
                if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    Vector3 posDelta = thisCamera.ScreenToWorldPoint(Input.mousePosition) -
                                       thisCamera.ScreenToWorldPoint(lastMousePos);
                    lastMousePos = Input.mousePosition;
                    transform.position -= posDelta;
                }
                float zoom = Input.mouseScrollDelta.y * zoomRate;
                thisCamera.orthographicSize -= zoom;
                thisCamera.orthographicSize = Mathf.Max(0.1f, thisCamera.orthographicSize);
                Vector3 tempPos = transform.position - Vector3.up * (zoom * 1f);
                tempPos.y = Mathf.Max(1.1f,tempPos.y);
                transform.position = tempPos;

            }
        }
    }

}