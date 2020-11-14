using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeViewer.Maze
{
    public enum MazeState
    {
        /// <summary>
        /// 墙
        /// </summary>
        Wall = 1,
        /// <summary>
        /// 路
        /// </summary>
        Route = 2,
        /// <summary>
        /// 入口
        /// </summary>
        Entry = 4,
        /// <summary>
        /// 出口
        /// </summary>
        Exit = 8
    }
    public class CellObj : MonoBehaviour
    {
        [SerializeField] private GameObject wall;
        [SerializeField] private GameObject entry;
        [SerializeField] private GameObject exit;

        public void SetState(MazeState state)
        {
            switch (state)
            {
                case MazeState.Wall:
                    wall.SetActive(true);
                    exit.SetActive(false);
                    entry.SetActive(false);
                    break;
                case MazeState.Route:
                    wall.SetActive(false);
                    exit.SetActive(false);
                    entry.SetActive(false);
                    break;
                case MazeState.Entry:
                    wall.SetActive(false);
                    exit.SetActive(false);
                    entry.SetActive(true);
                    break;
                case MazeState.Exit:
                    wall.SetActive(false);
                    exit.SetActive(true);
                    entry.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }

}
