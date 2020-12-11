using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeViewer.Viewer
{
    public class TextAligner : MonoBehaviour
    {
        [SerializeField] private Transform anchor = default;
        [SerializeField] private int availableDistance = 10;
        private MeshRenderer[] renderers;
        /// <summary>
        /// 到摄像头的曼哈顿距离
        /// </summary>
        private int DistanceToCamera
        {
            get
            {
                var delta = transform.position - Camera.main.transform.position;
                return (int) (Mathf.Abs(delta.x) + Mathf.Abs(delta.y) + Mathf.Abs(delta.z));
            }
        }
        /// <summary>
        /// 获取要对齐的方向
        /// </summary>
        /// <returns></returns>
        private Vector3 ToAlignDirection()
        {
            Vector3 sight = Camera.main.transform.up;
            if(Mathf.Abs(sight.x) > Mathf.Abs(sight.z))
            {
                return new Vector3(sight.x > 0 ? 1 : -1, 0, 0);
            }
            else
            {
                return new Vector3(0, 0, sight.z > 0 ? 1 : -1);
            }
        }

        private void Awake()
        {
            renderers = GetComponentsInChildren<MeshRenderer>();
        }

        [ExecuteInEditMode]
        private void Update()
        {
            if(DistanceToCamera <= availableDistance)
            {
                foreach (var meshRenderer in renderers)
                {
                    meshRenderer.enabled = true;
                }
                Vector3 current = anchor.forward;
                Quaternion rotate = Quaternion.FromToRotation(current, ToAlignDirection());
                transform.rotation *= rotate;
            }
            else
            {
                foreach (var meshRenderer in renderers)
                {
                    meshRenderer.enabled = false;
                }
            }
        }
    }

}