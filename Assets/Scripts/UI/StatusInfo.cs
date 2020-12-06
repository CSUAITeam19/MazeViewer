using System;
using System.Collections;
using System.Collections.Generic;
using MazeViewer.Viewer;
using UnityEngine;
using UnityEngine.UI;

namespace MazeViewer.UI
{
    /// <summary>
    /// 状态信息
    /// <para>单例</para>
    /// </summary>
    public class StatusInfo : MonoBehaviour
    {
        private List<string> infoList = new List<string>();
        private List<IEnumerator> timerList = new List<IEnumerator>();
        [SerializeField] private Text thisText = default;
        public static StatusInfo Instance { get; private set; }

        /// <summary>
        /// 最大信息数
        /// </summary>
        public int MaxInfoCount
        {
            get => ConfigManager.instance.Current.maxInfoCount;
        }

        /// <summary>
        /// 一条信息的最大生命周期
        /// </summary>
        public float InfoLifeTime
        {
            get => ConfigManager.instance.Current.infoLifeTime;
        }

        /// <summary>
        /// 清除一条信息, TODO: 在这之前使文字逐渐暗淡
        /// </summary>
        /// <returns></returns>
        private IEnumerator InfoKiller()
        {
            yield return new WaitForSeconds(InfoLifeTime);
            if (infoList.Count > 0) infoList.RemoveAt(0);
            RePrintInfo();
        }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else if(Instance != this)
            {
                Debug.LogError("Can not have duplicated StatusInfo in one scene!");
            }
            if(thisText == null)
            {
                thisText = GetComponent<Text>();
            }
            if(thisText == null)
            {
                Debug.LogError("Can not found Text Component!");
            }
            else
            {
                PrintInfo("程序已启动");
                PrintInfo("状态栏初始化完毕");
            }
        }

        private void RePrintInfo()
        {
            string temp = "";
            for(int i = 0; i < infoList.Count - 1; i++)
            {
                temp += infoList[i] + "\n";
            }

            if(infoList.Count > 0)
                temp += infoList[infoList.Count - 1];
            thisText.text = temp;
        }

        public void PrintInfo(string info)
        {
            infoList.Add(info);
            // 溢出则去除第一个
            if(infoList.Count > MaxInfoCount)
            {
                infoList.RemoveAt(0);
                StopCoroutine(timerList[0]);
                timerList.RemoveAt(0);
            }
            var timer = InfoKiller();
            timerList.Add(timer);
            StartCoroutine(timer);
            RePrintInfo();
        }

        /// <summary>
        /// 打印错误, 显示为红色
        /// </summary>
        /// <param name="error"></param>
        public void PrintError(string error)
        {
            PrintInfo(error.AddColor(Color.red));
        }
    }
}