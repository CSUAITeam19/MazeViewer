using System;
using UnityEngine;
using MazeViewer.Maze;
using Unitilities.Effect;

namespace MazeViewer.Viewer
{
    public class RouteCell : BasicCell
    {

        [SerializeField] private ColorChange colorChanges = default;
        [SerializeField] private GameObject dataText = default;
        [SerializeField] private TextMesh  cost = default, h = default;

        public override MazeState State => MazeState.Route;

        /// <summary>
        /// 刷新搜索数据
        /// </summary>
        private void RefreshData()
        {
            cost.text = searchData.cost.ToString();
            h.text = searchData.h.ToString();
        }

        public override void Init()
        {
            BackToIdle();
        }

        /// <summary>
        /// 播放开启特效
        /// </summary>
        private void StartOpenEffect()
        {
            dataText.SetActive(true);
            colorChanges.ResetColor(Color.green);
            colorChanges.Play();
        }

        /// <summary>
        /// 播放关闭特效
        /// </summary>
        private void StartCloseEffect()
        {
            dataText.SetActive(true);
            colorChanges.ResetColor(Color.gray);
            colorChanges.Play();
        }

        /// <summary>
        /// 立即转变为Idle状态
        /// </summary>
        private void BackToIdle()
        {
            colorChanges.ResetColor(Color.white);
            colorChanges.Stop();
            dataText.SetActive(false);
        }

        public override void UpdateSearchState(CellSearchData searchData)
        {
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
