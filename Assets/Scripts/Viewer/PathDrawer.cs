using System;
using System.Collections;
using System.Collections.Generic;
using Unitilities;
using UnityEngine;

namespace MazeViewer.Viewer
{
    public class PathDrawOperation :IRecovableOperation
    {
        private List<Vector2Int> path;
        private PathDrawer drawer;
        public PathDrawOperation(List<Vector2Int> path, PathDrawer drawer)
        {
            this.path = path;
            this.drawer = drawer;
        }
        public void Execute()
        {
            drawer.ShowPath(path);
        }

        public void Undo()
        {
            drawer.HidePath();
        }

        public bool Merge(IRecovableOperation operation)
        {
            return false;
        }
    }
    /// <summary>
    /// 路径绘制器
    /// </summary>
    public class PathDrawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer line;
        [SerializeField] private Transform baseTransform;

        private void Awake()
        {
            // 初始化引用, null则指定为自身的组件
            if(line == null)
            {
                line = GetComponent<LineRenderer>();
                if(line == null)
                {
                    Debug.LogError("line renderer not assigned! Can not draw path in the future.");
                }
            }
            if(baseTransform == null)
            {
                baseTransform = transform;
            }
            HidePath();
        }

        /// <summary>
        /// 显示路径
        /// </summary>
        /// <param name="path">需显示的路径, 以Vector2Int为元素</param>
        public void ShowPath(List<Vector2Int> path)
        {
            if(line != null)
            {
                List<Vector3> worldPath = path.ConvertAll(intPos =>
                        baseTransform.localToWorldMatrix.MultiplyPoint(new Vector3(intPos.x, 0, intPos.y))
                    );
                // worldPath.ForEach(pos => Debug.Log($"converted pos: {pos}"));
                worldPath[0] += Vector3.up * 0.1f;
                worldPath[worldPath.Count - 1] -= Vector3.up * .1f;
                line.positionCount = worldPath.Count;
                line.SetPositions(worldPath.ToArray()); 
            }
        }

        /// <summary>
        /// 隐藏路径: 实际上是删除所有记录
        /// </summary>
        public void HidePath()
        {
            if(line != null)
            {
                line.positionCount = 0;
            }
        }
    } 
}
