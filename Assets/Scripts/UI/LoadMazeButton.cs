using System.Collections;
using System.Collections.Generic;
using MazeViewer.Viewer;
using UnityEngine;

public class LoadMazeButton : MonoBehaviour
{
    public Container container;

    /// <summary>
    /// 载入迷宫
    /// </summary>
    public void Load()
    {
        container.LoadMaze();
    }
}
