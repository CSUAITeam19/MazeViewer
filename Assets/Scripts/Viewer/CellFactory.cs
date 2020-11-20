using System;
using MazeViewer.Maze;
using Unitilities;
using UnityEngine;

namespace MazeViewer.Viewer
{
    public class CellFactory : MonoBehaviour
    {
        [SerializeField] private GameObject entryMarker;
        [SerializeField] private GameObject exitMarker;

        /// <summary>
        /// 墙物品池
        /// </summary>
        [SerializeField] private GameObjectPool wallPool;
        /// <summary>
        /// 路径物品池
        /// </summary>
        [SerializeField] private GameObjectPool routePool;

        /// <summary>
        /// 入口标识
        /// </summary>
        public GameObject EntryMarker => entryMarker;
        /// <summary>
        /// 出口标识
        /// </summary>
        public GameObject ExitMarker => exitMarker;

        /// <summary>
        /// 获取迷宫墙物品
        /// </summary>
        /// <param name="pos">坐标</param>
        /// <returns></returns>
        private GameObject GetWallObj(Vector2Int pos)
        {
            GameObject temp = null;
            wallPool.Pop(out temp);
            return temp;
        }

        /// <summary>
        /// 获取路径物品
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private GameObject GetRouteObj(Vector2Int pos)
        {
            GameObject temp = null;
            routePool.Pop(out temp);
            return temp;
        }

        /// <summary>
        /// 根据初始参数提供ICellObj
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public GameObject GetCellObj(Vector2Int pos, MazeState state)
        {
            GameObject temp = null;
            switch (state)
            {
                case MazeState.Wall:
                    temp = GetWallObj(pos);
                    break;
                case MazeState.Route:
                    temp = GetRouteObj(pos);
                    break;;
                case MazeState.Entry:
                    temp = EntryMarker;
                    break;
                case MazeState.Exit:
                    temp = ExitMarker;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            temp.GetComponent<ICell>().CellPosition = pos;
            return temp;
        }
    } 
}
