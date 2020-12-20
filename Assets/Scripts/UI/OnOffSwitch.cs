using System.Collections.Generic;
using UnityEngine;
namespace MazeViewer.UI
{
    /// <summary>
    /// 显示/关闭UI的开关按钮
    /// </summary>
    public class OnOffSwitch : MonoBehaviour
    {
        public bool isOn = true;

        /// <summary>
        /// 开启时的Pivot
        /// </summary>
        public Vector2 onPivot;
        /// <summary>
        /// 关闭时的Pivot
        /// </summary>
        public Vector2 offPivot;
        /// <summary>
        /// 同步显示的目标队列
        /// </summary>
        public List<GameObject> syncTargets = new List<GameObject>();
        /// <summary>
        /// 同步反向显示的目标队列, 开关开启时隐藏, 关闭时显示
        /// </summary>
        public List<GameObject> inversedSyncTargets = new List<GameObject>();

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        public void SwitchState()
        {
            if(isOn)
            {
                rectTransform.pivot = offPivot;
                isOn = false;
                syncTargets.ForEach(each =>
                {
                    if (each != null) 
                        each.SetActive(false);
                });
                inversedSyncTargets.ForEach(each =>
                {
                    if(each != null)
                        each.SetActive(true);
                });
            }
            else
            {
                rectTransform.pivot = onPivot;
                isOn = true;
                syncTargets.ForEach(each =>
                {
                    if(each != null)
                        each.SetActive(true);
                });
                inversedSyncTargets.ForEach(each =>
                {
                    if(each != null)
                        each.SetActive(false);
                });
            }
        }
    } 
}
