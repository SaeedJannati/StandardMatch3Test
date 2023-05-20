namespace Match3.EventController
{
    interface IEventListener
    {
        void RegisterToEvents();

        void UnregisterFromEvents();
    }
}