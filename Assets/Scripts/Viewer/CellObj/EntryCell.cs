using MazeViewer.Maze;
using UnityEngine;

namespace MazeViewer.Viewer
{
    public class EntryCell : BasicCell
    {
        public override MazeState State => MazeState.Entry;

        [SerializeField] private ParticleSystem effect = default;

        private void Start()
        {
            effect.Play();
        }

        public override void UpdateSearchState(CellSearchData searchData)
        {
            this.searchData = searchData;
            switch (searchData.state)
            {
                case SearchState.Idle:
                    effect.Play();
                    break;
                case SearchState.Opened:
                    effect.Stop();
                    break;
                case SearchState.Closed:
                    effect.Stop();
                    break;
            }
        }
    }

}