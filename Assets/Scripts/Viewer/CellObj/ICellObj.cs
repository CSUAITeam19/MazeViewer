using System.Collections;
using System.Collections.Generic;
using MazeViewer.Maze;
using UnityEngine;

namespace MazeViewer.Viewer
{
    public interface ICellObj : ICell
    {
        /// <summary>
        /// 初始化状态, 使状态回归到无活动
        /// </summary>
        void Init();

        /// <summary>
        /// 更新搜索单元数据
        /// </summary>
        /// <param name="searchData"></param>
        void UpdateSearchState(CellSearchData searchData);

        /// <summary>
        /// 搜索数据
        /// </summary>
        CellSearchData SearchData { get; }
    }
}