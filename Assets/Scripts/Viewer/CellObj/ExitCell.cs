using MazeViewer.Maze;
using UnityEngine;

namespace MazeViewer.Viewer
{
    public class ExitCell : BasicCell
    {
        public override MazeState State => MazeState.Exit;

        [SerializeField] private ParticleSystem effect;

        private void Start()
        {
            effect.Stop();
        }

        public override void UpdateSearchState(CellSearchData searchData)
        {
            this.searchData = searchData;
            switch(searchData.state)
            {
                case SearchState.Idle:
                    effect.Stop();
                    break;
                case SearchState.Opened:
                    effect.Play();
                    break;
                case SearchState.Closed:
                    break;
            }
        }
    }
}
