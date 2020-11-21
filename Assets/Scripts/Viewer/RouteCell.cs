using System;
using UnityEngine;
using MazeViewer.Maze;

namespace MazeViewer.Viewer
{
    public class RouteCell : BasicCell
    {

        // temp implement
        [SerializeField] private MeshRenderer meshRenderer;

        public override Vector2Int CellPosition
        {
            get => cellPosition;
            set
            {
                cellPosition = value;
                transform.localPosition = new Vector3(value.x, -0.5f, value.y);
            }
        }

        public override MazeState State => MazeState.Route;

        /// <summary>
        /// 刷新搜索数据
        /// </summary>
        private void RefreshData()
        {
            // TODO: show 3d text
            Debug.Log($"{cellPosition}: cost = {searchData.cost}, h = {searchData.h}");
        }

        

        /// <summary>
        /// 播放开启特效
        /// </summary>
        private void StartOpenEffect()
        {
            meshRenderer.material.color = Color.green;
        }

        /// <summary>
        /// 播放关闭特效
        /// </summary>
        private void StartCloseEffect()
        {
            meshRenderer.material.color = Color.gray;
        }

        /// <summary>
        /// 直接转变为Idle状态, 没有动画
        /// </summary>
        private void BackToIdle()
        {
            meshRenderer.material.color = Color.white;
        }

        public override void UpdateSearchState(CellSearchData searchData)
        {
            // TODO: 返回到未搜索的状态
            if(searchData.state != this.searchData.state)
            {
                switch (searchData.state)
                {
                    case SearchState.Opened:
                        StartOpenEffect();
                        break;
                    case SearchState.Closed:
                        StartCloseEffect();
                        break;
                    case SearchState.Idle:
                        BackToIdle();
                        break;
                }
            }
            this.searchData = searchData;
            RefreshData();
        }
    }
}
