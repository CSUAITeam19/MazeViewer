using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MazeViewer.UI
{
    /// <summary>
    /// 状态栏
    /// </summary>
    public class StatusBar : MonoBehaviour
    {
        [SerializeField] private Text mouseOnPos = default;
        [SerializeField] private Text finalCostText = default;

        public static StatusBar Instance { get; private set; }

        private void Awake()
        {
            if(mouseOnPos == null)
            {
                Debug.LogError("mouseOnPosX  is null!");
            }
            else
            {
                mouseOnPos.text = "";
            }
            if(finalCostText == null)
            {
                Debug.LogError("finalCost is null!");
            }
            else
            {
                finalCostText.text = "";
            }
            if(Instance == null)
            {
                Instance = this;
            }
            else if(Instance != this)
            {
                Debug.LogError("Can not have duplicated StatusBar!");
            }
        }

        public void MouseOnCell(Vector2Int pos)
        {
            mouseOnPos.text = $"当前坐标: {pos}";
        }

        public void MouseLeaveCell()
        {
            mouseOnPos.text = "";
        }

        public void SetFinalCost(int finalCost)
        {
            finalCostText.text = $"总代价: {finalCost}";
        }
    }

}