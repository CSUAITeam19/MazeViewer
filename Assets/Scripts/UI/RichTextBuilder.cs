using UnityEngine;

namespace MazeViewer.UI
{
    public static class RichTextBuilder
    {
        public static string AddColor(this string str, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{str}</color>";
        }
    }
}