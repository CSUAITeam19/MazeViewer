using System.Collections;
using System.Collections.Generic;
using MazeViewer.Maze;
using UnityEngine;

namespace MazeViewer.Viewer
{
    /// <summary>
    /// 所有基本单元的统一实现
    /// </summary>
    public class BasicCell : MonoBehaviour,ICellObj
    {
        protected Vector2Int cellPosition;

        public virtual Vector2Int CellPosition
        {
            get => cellPosition;
            set
            {
                cellPosition = value;
                transform.localPosition = new Vector3(cellPosition.x, 0, cellPosition.y);
            }
        }
    
        public virtual MazeState State => MazeState.Wall;

        public virtual void Init()
        {
            
        }

        public virtual void UpdateSearchState(SearchState searchState)
        {
            
        }
    }

}