using System.Collections;
using System.Collections.Generic;
using MazeViewer.Maze;
using UnityEngine;

namespace MazeViewer.Viewer
{
    public class EntryCell : BasicCell
    {
        public override MazeState State => MazeState.Entry;
    }

}