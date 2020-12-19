using System;
using UnityEngine;
using MazeViewer.Maze;
using MazeViewer.UI;
using Unitilities.Effect;

namespace MazeViewer.Viewer
{
    public class RouteCell : BasicCell
    {
        [SerializeField] private GameObject dataText = default;
        [SerializeField] private TextMesh  cost = default, h = default;
        [SerializeField] private MeshRenderer targetRenderer = default;
        private bool changeFlag = false;
        private SearchState nextState = SearchState.Idle;
        public Material idleMaterial, openMaterial, closedMaterial;
        public override MazeState State => MazeState.Route;

        private void Awake()
        {
            if (targetRenderer == null) targetRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            // 有变化再修改, 否则啥都不干
            if(changeFlag)
            {
                switch (nextState)
                {
                    case SearchState.Idle:
                        BackToIdle();
                        break;
                    case SearchState.Opened:
                        StartOpenEffect();
                        break;
                    case SearchState.Closed:
                        StartCloseEffect();
                        break;
                }
                changeFlag = false;
            }
        }

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
            if(targetRenderer != null)
                targetRenderer.material = openMaterial;
        }

        /// <summary>
        /// 播放关闭特效
        /// </summary>
        private void StartCloseEffect()
        {
            dataText.SetActive(true);
            if(targetRenderer != null)
                targetRenderer.material = closedMaterial;
        }

        /// <summary>
        /// 立即转变为Idle状态
        /// </summary>
        private void BackToIdle()
        {
            dataText.SetActive(false);
            if(targetRenderer != null)
                targetRenderer.material = idleMaterial;
        }
        public override void UpdateSearchState(CellSearchData searchData)
        {
            if(searchData.state != this.searchData.state)
            {
                nextState = searchData.state;
                changeFlag = true;
            }
            this.searchData = searchData;
            RefreshData();
        }
    }
}
