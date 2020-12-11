using System;
using MazeViewer.Maze;
using Unitilities.Effect;
using UnityEngine;

namespace MazeViewer.Viewer
{
    
    public class WallCell : BasicCell
    {
        private IEffect showUpEffect;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            //if(showUpEffect == null)
            //{
            //    showUpEffect = GetComponent<ShowUp>();
            //}
            //showUpEffect.Play();
        }

        public override MazeState State => MazeState.Wall;
    }
}
