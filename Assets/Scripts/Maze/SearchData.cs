using System;
using System.Collections.Generic;
using MazeViewer.Viewer;
using Unitilities;

namespace MazeViewer.Maze
{
    /// <summary>
    /// 搜索状态
    /// </summary>
    public enum SearchState
    {
        Idle,
        Opened,
        Closed
    }
    /// <summary>
    /// 一个单元上的搜索数据
    /// </summary>
    [Serializable]
    public struct CellSearchData
    {
        /// <summary>
        /// 当前搜索状态
        /// </summary>
        public SearchState state;
        /// <summary>
        /// 已经计算出的代价
        /// </summary>
        public int cost;
        /// <summary>
        /// 对于A*算法, h为预测值
        /// </summary>
        public int h;
        public CellSearchData(SearchState state, int cost, int h)
        {
            this.state = state;
            this.cost = cost;
            this.h = h;
        }
        public static readonly CellSearchData defaultData = new CellSearchData(SearchState.Idle,0,0);
        public override string ToString()
        {
            return $"State: {state}, cost: {cost}, h: {h}";
        }
    }
}
