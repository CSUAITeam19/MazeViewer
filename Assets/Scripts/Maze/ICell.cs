using MazeViewer.Viewer;
using UnityEngine;

namespace MazeViewer.Maze
{
    public enum MazeState
    {
        /// <summary>
        /// 墙
        /// </summary>
        Wall = 1,
        /// <summary>
        /// 路
        /// </summary>
        Route = 2,
        /// <summary>
        /// 入口
        /// </summary>
        Entry = 4,
        /// <summary>
        /// 出口
        /// </summary>
        Exit = 8
    }
    /// <summary>
    /// 迷宫单元
    /// </summary>
    public interface ICell
    {
        Vector2Int CellPosition
        {
            get; set;
        }
        MazeState State { get; }
    }
}
