using System.Collections.Generic;
using MazeViewer.Maze;
using Unitilities;
using UnityEngine;

namespace MazeViewer.Viewer
{
    /// <summary>
    /// 搜索元操作: 对一个单元的 Open/Close 操作
    /// </summary>
    public class MetaSearchOperation : IRecovableOperation
    {
        private readonly ICellObj target;
        private readonly CellSearchData lastData;
        private readonly CellSearchData nextData;

        public MetaSearchOperation(ICellObj target, CellSearchData nextData)
        {
            this.target = target;
            lastData = target.SearchData;
            this.nextData = nextData;
        }

        public void Execute()
        {
            target.UpdateSearchState(nextData);
        }

        public void Undo()
        {
            target.UpdateSearchState(lastData);
        }

        public bool Merge(IRecovableOperation operation)
        {
            return false;
        }
    }
    /// <summary>
    /// 生成单个元操作
    /// </summary>
    public static class MetaSearchOperationFactory
    {
        public static MetaSearchOperation MakeOpenOperation(ICellObj target, int cost, int h)
        {
            return new MetaSearchOperation(target, new CellSearchData(SearchState.Opened, cost, h));
        }
        public static MetaSearchOperation MakeCloseOperation(ICellObj target, int cost, int h)
        {
            return new MetaSearchOperation(target, new CellSearchData(SearchState.Closed, cost, h));
        }
        public static MetaSearchOperation MakeCostRefreshOperation(ICellObj target, int cost)
        {
            CellSearchData data = target.SearchData;
            data.cost = cost;
            return new MetaSearchOperation(target, data);
        }
    }
    /// <summary>
    /// 搜索步骤: 由若干搜索元操作组成
    /// </summary>
    public class StepOperation:IRecovableOperation
    {
        private List<IRecovableOperation> searchOperList = new List<IRecovableOperation>();

        /// <summary>
        /// 添加新的元操作
        /// </summary>
        /// <param name="metaOperation">待添加的操作</param>
        public void Add(MetaSearchOperation metaOperation)
        {
            searchOperList.Add(metaOperation);
        }

        public void Execute()
        {
            // 按添加顺序执行
            for(int i = 0; i < searchOperList.Count; i++)
            {
                searchOperList[i].Execute();
            }
        }

        public void Undo()
        {
            // 按添加顺序倒序撤销
            for(int i = searchOperList.Count - 1; i >= 0; i--)
            {
                searchOperList[i].Undo();
            }
        }

        public bool Merge(IRecovableOperation operation)
        {
            // 不允许合并
            return false;
        }
    }
}

