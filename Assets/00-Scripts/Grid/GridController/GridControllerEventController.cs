using Match3.EventController;

namespace Match3.General
{
    public class GridControllerEventController:BaseEventController
    {
        public readonly SimpleEvent onCreateGridRequest = new();
        public readonly ListFuncEvent<TilesGrid> onGridRequest = new();
    }
}