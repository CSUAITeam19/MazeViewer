using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeViewer.Viewer;
using UnityEngine.UI;

namespace MazeViewer.UI
{
    public class MazePathInput : MonoBehaviour
    {
        private InputField input;

        private void Awake()
        {
            input = GetComponent<InputField>();
        }

        public void UpdateMazePath(Container container)
        {
            container.UpdateMazePath(input.text);
        }
    }

}