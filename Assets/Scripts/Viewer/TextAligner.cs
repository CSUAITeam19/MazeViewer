using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeViewer.Viewer
{
    public class TextAligner : MonoBehaviour
    {
        [SerializeField] private Transform anchor;
        
        /// <summary>
        /// 获取要对齐的方向
        /// </summary>
        /// <returns></returns>
        private Vector3 ToAlignDirection()
        {
            Vector3 sight = Camera.main.transform.forward;
            if(Mathf.Abs(sight.x) > Mathf.Abs(sight.z))
            {
                return new Vector3(sight.x > 0 ? 1 : -1, 0, 0);
            }
            else
            {
                return new Vector3(0, 0, sight.z > 0 ? 1 : -1);
            }
        }

        [ExecuteInEditMode]
        private void Update()
        {
            Vector3 current = anchor.forward;
            Quaternion rotate = Quaternion.FromToRotation(current,ToAlignDirection());
            transform.rotation *= rotate;
        }
    }

}