using Arixen.ScriptSmith;

namespace Yuddham
{
    public class HighlightGridCubeEvent : IGameEventData
    {
        public GridCube _selectedCube;

        public HighlightGridCubeEvent(GridCube selectedCube)
        {
            _selectedCube = selectedCube;
        }
    }
}
