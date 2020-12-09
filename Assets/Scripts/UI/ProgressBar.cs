using System;
using System.Collections;
using System.Collections.Generic;
using MazeViewer.Viewer;
using UnityEngine;
using UnityEngine.UI;

namespace MazeViewer.UI
{
    /// <summary>
    /// 进度条
    /// </summary>
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private ProgressMgr progressMgr = default;
        [SerializeField] private Slider slider = null;

        private void Awake()
        {
            if(slider == null)
            {
                slider = GetComponent<Slider>();
            }
            if(slider == null)
            {
                Debug.LogError("Slider not found! Will not run as expected.");
                return;
            }
            if(progressMgr == null)
            {
                Debug.LogError("progressMgr is null here. Will not run as expected.");
                return;
            }

            progressMgr.postLoadChainEvent += UpdateScrollBarStep;
            progressMgr.stepChangeEvent += UpdateProgressBar;
        }

        private void UpdateScrollBarStep()
        {
            slider.wholeNumbers = true;
            slider.minValue = 0;
            slider.maxValue = progressMgr.CurrentStep;
        }

        /// <summary>
        /// 从脚本更新进度条, 与用户拖动区分
        /// </summary>
        public void UpdateProgressBar(int step)
        {
            slider.SetValueWithoutNotify(step);
        }

        /// <summary>
        /// 用户拖动进度条
        /// </summary>
        public void BarValueChangedEvent()
        {
            progressMgr.JumpToStep((int) slider.value);
        }
    }

}