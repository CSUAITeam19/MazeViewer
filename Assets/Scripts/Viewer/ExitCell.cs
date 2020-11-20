using MazeViewer.Maze;
using UnityEngine;

namespace MazeViewer.Viewer
{
    public class ExitCell : BasicCell
    {
        public override MazeState State => MazeState.Exit;
    }
}
