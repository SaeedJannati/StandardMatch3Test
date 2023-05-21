using System;
using System.Linq;
using Match3.Auxiliary;
using Match3.EventController;
using Zenject;

namespace Match3.General.MoveTest
{
    public class RandomMoveApplier:IDisposable,IEventListener,IInitializable
    {
        #region Fields

        [Inject] private GridControllerEventController _gridEventController;
        [Inject] private MoveTestEventController _testEventController;
        private TilesGrid _grid;
        

        #endregion

        #region Methods
        public void Initialize()
        {
            RegisterToEvents();
        }

        public void RegisterToEvents()
        {
            _gridEventController.onGridCreated.Add(OnGridCreated);
            _gridEventController.onRandomMoveRequest.Add(OnRandomMoveRequest);
        }

        public void UnregisterFromEvents()
        {
            _gridEventController.onGridCreated.Remove(OnGridCreated);
            _gridEventController.onRandomMoveRequest.Remove(OnRandomMoveRequest);
        }

        private void OnGridCreated()
        {
            _grid = _gridEventController.onGridRequest.GetFirstNonDefaultResult();
        }

        private void OnRandomMoveRequest()
        {
         var indices=Enumerable.Range(0, _grid.count).ToList();
         indices.Shuffle();
         foreach (var index in indices)
         {
             var info=_gridEventController.onCheckForPossibleMove.GetFirstResult(_grid[index]);
             if(!info.possible)
                 continue;
             _gridEventController.onSwipeRequest.Trigger((_grid[index].row,_grid[index ].col,info.swipeDirection));
             return;
         }
        }

        public void Dispose()
        {
            UnregisterFromEvents();
            GC.SuppressFinalize(this);
        }
        #endregion


       
    }
}

