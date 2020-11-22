using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MazeViewer.UI
{
    /// <summary>
    /// /提交即退出焦点的组件
    /// </summary>
    public class SubmitExit : MonoBehaviour
    {
        public void ExitFocus()
        {
            if(EventSystem.current.currentSelectedGameObject == gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

}