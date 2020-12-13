using System;
using System.Collections;
using System.Collections.Generic;
using MazeViewer.Viewer;
using UnityEngine;

namespace MazeViewer.UI
{
    /// <summary>
    /// 文本对齐向导
    /// </summary>
    public class TextAlignAgent : MonoBehaviour
    {
        public static TextAlignAgent Instance;

        private HashSet<TextAligner> aligners = new HashSet<TextAligner>();
        private Vector3 toAlign = Vector3.forward;
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            if(Instance != this)
            {
                Debug.LogError("Can not have duplicated TextAlignAgent!");
            }
        }

        private void Update()
        {
            var nextDirection = ToAlignDirection();
            if((toAlign - nextDirection).sqrMagnitude > 1f)
            {
                toAlign = nextDirection;
                UpdateAll();
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

        private void UpdateAll()
        {
            foreach (var textAligner in aligners)
            {
                textAligner.UpdateDirection(toAlign);
            }
        }

        public void AddAligner(TextAligner toAdd)
        {
            if(!aligners.Contains(toAdd))
            {
                aligners.Add(toAdd);
                toAdd.UpdateDirection(toAlign);
            }
        }

        public void RemoveAligner(TextAligner toRemove)
        {
            if(aligners.Contains(toRemove))
            {
                aligners.Remove(toRemove);
            }
        }
    }

}