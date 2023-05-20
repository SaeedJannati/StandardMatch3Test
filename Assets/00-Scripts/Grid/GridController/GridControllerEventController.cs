using Match3.EventController;

namespace Match3.General
{
    public class GridControllerEventController:BaseEventController
    {
        public readonly SimpleEvent onCreateGridRequest = new();
        public readonly ListFuncEvent<TilesGrid> onGridRequest = new();
        public readonly SimpleEvent onShuffleRequest=new();
        public readonly ListEvent<(int row, int col, Direction dir)> onSwipeRequest = new();
        public readonly SimpleEvent onGridCreated = new();
    }
}