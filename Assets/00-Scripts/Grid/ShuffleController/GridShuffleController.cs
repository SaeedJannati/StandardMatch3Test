using System;
using System.Linq;
using Match3.Auxiliary;
using Match3.EventController;
using Zenject;

namespace Match3.General
{
    public class GridShuffleController : IDisposable, IEventListener
    {
        #region Fields

        [Inject] private GridControllerEventController _eventController;
        [Inject] private MatchCheckerEventController _matchEventController;
        private TilesGrid _grid;
        #endregion

        #region Methods

        [Inject]
        void Initialise()
        {
            RegisterToEvents();
        }

        public void Dispose()
        {
            UnregisterFromEvents();
            GC.SuppressFinalize(this);
        }

        public void RegisterToEvents()
        {
            _eventController.onGridCreated.Add(OnGridCreated);
            _eventController.onCheckForPossibleMove.Add(OnCheckForPossibleMove);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onGridCreated.Remove(OnGridCreated);
            _eventController.onCheckForPossibleMove.Remove(OnCheckForPossibleMove);
        }

        private (bool possible, Direction swipeDirection) OnCheckForPossibleMove(TileGridElement element)
        {
            if (IsSwipeDownMatch(element))
                return (true, Direction.Down);
            if(IsSwipeRightMatch(element))
                return (true, Direction.Right);
            return (false, default);
        }

        private void OnGridCreated()
        {
            _grid = _eventController.onGridRequest.GetFirstResult();
        }

        public bool OnShuffleNeedCheck()
        {
            if(CheckForPossibleMatch())
                return false;
            ShuffleGrid();
            return true;
        }

        public async void ShuffleGrid()
       {
           var values = _grid.elements.Select(i => i.value).ToList();
           values.Shuffle();
           for (int i = 0, e = _grid.count; i < e; i++)
           {
               _grid[i].SetValue(values[i], false);
           }

           _eventController.onShuffleEffectRequest.Trigger();
       
       }

        bool CheckForPossibleMatch()
       {
           foreach (var element in _grid.elements)
           {
               if (IsPossibleMatch(element))
                   return true;
           }

           return false;
       }

       bool IsPossibleMatch(TileGridElement element)
       {
           if (IsSwipeRightMatch(element))
               return true;
           if (IsSwipeDownMatch(element))
               return true;
           return false;
       }

       bool IsSwipeRightMatch(TileGridElement element)
       {
           if (element.col == _grid.columns - 1)
               return false;
           var rightElement = _grid[element.row, element.col+1];
           var isPossible=CheckMatchThenSwapElements(element, rightElement);
           return isPossible;
       }

       bool IsSwipeDownMatch(TileGridElement element)
       {
           if (element.row == _grid.rows - 1)
               return false;
           var downElement = _grid[element.row + 1, element.col];
           var isPossible=CheckMatchThenSwapElements(element, downElement);
           return isPossible;
       }

       bool CheckMatchThenSwapElements(TileGridElement first, TileGridElement second)
       {
           SwapElementValues(first, second);
           if (_matchEventController.onElementMatchCheck.GetFirstResult(first))
           {
               SwapElementValues(first,second);
               return true;
           }

           if (_matchEventController.onElementMatchCheck.GetFirstResult(second))
           {
               SwapElementValues(first,second);
               return true;
           }
           SwapElementValues(first, second);
           return false;
       }

       void SwapElementValues(TileGridElement first,TileGridElement second)
       {
           var value = first.value;
           first.SetValue(second.value,false);
           second.SetValue(value,false);
       }
       
       

       #endregion
    }
}