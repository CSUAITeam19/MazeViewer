using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeViewer.UI
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
        

        private void Awake()
        {
            renderers = GetComponentsInChildren<MeshRenderer>();
        }

        private void Update()
        {
            if(DistanceToCamera <= availableDistance)
            {
                foreach (var meshRenderer in renderers)
                {
                    meshRenderer.enabled = true;
                }
                TextAlignAgent.Instance.AddAligner(this);
            }
            else
            {
                foreach (var meshRenderer in renderers)
                {
                    meshRenderer.enabled = false;
                }
                TextAlignAgent.Instance.RemoveAligner(this);
            }
        }

        public void UpdateDirection(Vector3 toAlignDirection)
        {
            Vector3 current = anchor.forward;
            Quaternion rotate = Quaternion.FromToRotation(current, toAlignDirection);
            transform.rotation *= rotate;
        }
    }

}