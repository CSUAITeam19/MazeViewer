﻿using System;
using System.Collections;
using System.Collections.Generic;
using MazeViewer.UI;
using Unitilities;
using UnityEngine;
using Logger = MazeViewer.UI.Logger;

namespace MazeViewer.Viewer
{
    /// <summary>
    /// 进度控制器
    /// </summary>
    public class ProgressMgr : MonoBehaviour
    {
        private OperationChain chain = new OperationChain();
        /// <summary>
        /// 上一次调用的位置
        /// </summary>
        private int lastCallPos;
        private int playSpeed = 0;
        [SerializeField] private int speedChangeScale = 2;

        /// <summary>
        /// 到达最后一步时调用
        /// </summary>
        public event Action endEvent;
        /// <summary>
        /// 进度变化时调用, 先于endEvent与beginEvent
        /// <para>参数为新的step</para>
        /// </summary>
        public event Action<int> stepChangeEvent;
        /// <summary>
        /// 进度在最开始时调用
        /// </summary>
        public event Action beginEvent;
        /// <summary>
        /// 操作链加载后调用
        /// </summary>
        public event Action postLoadChainEvent;
        public int StepCount => chain.StoredOperCount;
        public int CurrentStep => chain.CurrentPos + 1;

        private void Awake()
        {
            endEvent += () =>
            {
                Logger.Instance.PrintInfo("搜索结束");
                playSpeed = 0;
            };
            
            beginEvent += () =>
            {
                Logger.Instance.PrintInfo("搜索开始");
                playSpeed = 0;
            };
            
        }

        private void Update()
        {
            if(playSpeed > 0)
            {
                NextSteps(playSpeed);
            }
            if(playSpeed < 0)
            {
                BackSteps(-playSpeed);
            }
        }

        private void CheckStepUpdate()
        {
            if(chain.CurrentPos != lastCallPos)
            {
                stepChangeEvent?.Invoke(CurrentStep);
                if(chain.IsBegin)
                {
                    beginEvent?.Invoke();
                }
                else if(chain.IsEnd)
                {
                    endEvent?.Invoke();
                }
            }

            lastCallPos = chain.CurrentPos;
        }

        /// <summary>
        /// 加载操作链
        /// </summary>
        /// <param name="chain"></param>
        public void LoadOperationChain(OperationChain chain)
        {
            this.chain = chain;
            lastCallPos = chain.CurrentPos;
            postLoadChainEvent?.Invoke();
        }

        /// <summary>
        /// 进度重置到0
        /// </summary>
        public void BackToBegin()
        {
            chain.UndoAll();
            CheckStepUpdate();
        }
        
        /// <summary>
        /// 下一步
        /// </summary>
        public void NextStep()
        {
            chain.Redo();
            CheckStepUpdate();
        }

        /// <summary>
        /// 下若干步
        /// </summary>
        /// <param name="step">步数</param>
        public void NextSteps(int step)
        {
            for(int i = 0; i < step && !chain.IsEnd; i++)
            {
                chain.Redo();
            }
            CheckStepUpdate();
        }

        /// <summary>
        /// 上一步
        /// </summary>
        public void BackStep()
        {
            chain.Undo();
            CheckStepUpdate();
        }

        /// <summary>
        /// 上若干步
        /// </summary>
        /// <param name="step"></param>
        public void BackSteps(int step)
        {
            for(int i = 0; i < step && !chain.IsBegin; i++)
            {
                chain.Undo();
            }
            CheckStepUpdate();
        }

        /// <summary>
        /// 跳到最后一步
        /// </summary>
        public void GoToEnd()
        {
            while (chain.Redo())
            {
            }
            CheckStepUpdate();
        }

        /// <summary>
        /// 跳到某一步
        /// </summary>
        /// <param name="target"></param>
        public void JumpToStep(int target)
        {
            if(target < CurrentStep)
            {
                BackSteps(CurrentStep - target);
            }
            else if(target > CurrentStep)
            {
                NextSteps(target - CurrentStep);
            }
        }

        /// <summary>
        /// 向前播放加速, 若当前速度为负则立即设置为正
        /// </summary>
        public void ForwardPlaySpeedUp()
        {
            if (playSpeed <= 0)
            {
                playSpeed = 1;
                return;
            }
            playSpeed *= speedChangeScale;
        }

        /// <summary>
        /// 向后播放加速, 若当前速度为正则立即设置为负
        /// </summary>
        public void BackwardPlaySpeedUp()
        {
            if (playSpeed >= 0)
            {
                playSpeed = -1;
                return;
            }
            playSpeed *= speedChangeScale;
        }

        /// <summary>
        /// 停止自动播放
        /// </summary>
        public void StopPlay()
        {
            playSpeed = 0;
        }
    }

}