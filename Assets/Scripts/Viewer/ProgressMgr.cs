using System;
using System.Collections;
using System.Collections.Generic;
using MazeViewer.UI;
using Unitilities;
using UnityEngine;

namespace MazeViewer.Viewer
{
    /// <summary>
    /// 进度控制器
    /// </summary>
    public class ProgressMgr : MonoBehaviour
    {
        private OperationChain chain = new OperationChain();

        /// <summary>
        /// 到达最后一步时调用
        /// </summary>
        public event Action endEvent;
        /// <summary>
        /// 进度在最开始时调用
        /// </summary>
        public event Action beginEvent;

        private void Awake()
        {
            endEvent += () =>  StatusInfo.Instance.PrintInfo("搜索结束");
            beginEvent += () => StatusInfo.Instance.PrintInfo("搜索开始");
        }

        /// <summary>
        /// 加载操作链
        /// </summary>
        /// <param name="chain"></param>
        public void LoadOperationChain(OperationChain chain)
        {
            this.chain = chain;
        }

        /// <summary>
        /// 进度重置到0
        /// </summary>
        public void BackToBegin()
        {
            chain.UndoAll();
            beginEvent?.Invoke();
        }
        
        /// <summary>
        /// 下一步
        /// </summary>
        public void NextStep()
        {
            if(!chain.Redo())
            {
                endEvent?.Invoke();
            }
        }

        /// <summary>
        /// 下若干步
        /// </summary>
        /// <param name="step">步数</param>
        public void NextSteps(int step)
        {
            for(int i = 0; i < step; i++)
            {
                if (!chain.Redo())
                {
                    endEvent?.Invoke();
                    break;
                }
            }
        }

        /// <summary>
        /// 上一步
        /// </summary>
        public void BackStep()
        {
            if(!chain.Undo())
            {
                beginEvent?.Invoke();
            }
        }

        /// <summary>
        /// 上若干步
        /// </summary>
        /// <param name="step"></param>
        public void BackSteps(int step)
        {
            for(int i = 0; i < step; i++)
            {
                if(!chain.Undo())
                {
                    beginEvent?.Invoke();
                    break;
                }
            }
        }

        /// <summary>
        /// 跳到最后一步
        /// </summary>
        public void GoToEnd()
        {
            while (chain.Redo())
            {
            }
            endEvent?.Invoke();
        }
    }

}