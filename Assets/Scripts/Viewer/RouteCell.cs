using UnityEngine;
using MazeViewer.Maze;

namespace MazeViewer.Viewer
{
    public class RouteCell : BasicCell
    {
        public override Vector2Int CellPosition
        {
            get => cellPosition;
            set
            {
                cellPosition = value;
                transform.localPosition = new Vector3(value.x, -0.5f, value.y);
            }
        }

        public override MazeState State => MazeState.Route;
    }
}
