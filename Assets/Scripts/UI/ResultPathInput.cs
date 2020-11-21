using UnityEngine;
using MazeViewer.Viewer;
using UnityEngine.UI;

namespace MazeViewer.UI
{
    public class ResultPathInput : MonoBehaviour
    {
        private InputField input;

        private void Awake()
        {
            input = GetComponent<InputField>();
        }

        public void UpdateResultPath(Container container)
        {
            container.UpdateResultPath(input.text);
        }
    }

}
