using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Match3.Auxiliary;
using Match3.EventController;
using Match3.General.MoveTest;
using TMPro;
using Zenject;

namespace Match3.General
{
    public class MatchChecker : IDisposable, IEventListener
    {
        #region Fields

        [Inject] private GridControllerEventController _gridEventController;
        [Inject] private MatchCheckerEventController _eventController;
        [Inject] private GridMoveEffectsModel _moveEffectsModel;
        [Inject] private MoveTestEventController _testEventController;
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
            _eventController?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void RegisterToEvents()
        {
            _eventController.onElementMatchCheck.Add(OnElementMatchCheck);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onElementMatchCheck.Add(OnElementMatchCheck);
        }

        private bool OnElementMatchCheck(TileGridElement element)=>IsPartOfMatch(element.row,element.col);
        

        public void CheckNeedToCheckElementsForMatch()
        {
            var elementsToCheck = _grid.elements.Where(i => i.needCheck).ToList();
            CheckElementsForMatch(elementsToCheck);
        }

        async Task<bool> CheckElementsForMatch(List<TileGridElement> elements)
        {
            var matchedElements = new List<TileGridElement>();
            foreach (var element in elements)
            {
                matchedElements.AddRange(CheckMatchForElement(element)) ;
            }

            if (matchedElements.Count==0)
                return false;
            await FadeTiles(matchedElements);
          
            _gridEventController.onAfterMatch.Trigger();
            return true;
        }

        async Task FadeTiles(List<TileGridElement> elements)
        {
            foreach (var aElement in elements)
            {
                aElement.SetValue(-1, false);
                _gridEventController.onTileViewFadeRequest.Trigger((aElement));
            }
         
            await CheckForDelay();
   
        }
        async Task CheckForDelay()
        {
            if(IsNonGraphicalTest())
                return;
            _gridEventController.onInputEnable.Trigger(false);
            await Task.Delay((int)(1000 * _moveEffectsModel.tileFadePeriod));
            _gridEventController.onInputEnable.Trigger(true);
        }

        bool IsNonGraphicalTest()
        {
            return _testEventController.onNonGraphicalTestRunningRequest.GetFirstResult();
        }
        public async void CheckFilledElements()
        {
            var elementsToCheck = _grid.elements.Where(i => i.value != -1).ToList();
            var isThereMatch = await CheckElementsForMatch(elementsToCheck);
            if (isThereMatch)
            {
                return;
            }

            _gridEventController.onFillEmptySlotsRequest.Trigger();
        
        }

        List<TileGridElement> CheckMatchForElement(TileGridElement element)
        {
            if (!IsPartOfMatch(element.row, element.col))
                return new();
            var elementsToFade =
                GetMatchedElements(element.row, element.col);
            return elementsToFade;
        }

        public bool IsPartOfMatch(int row, int col)
        {
            if (_grid[row, col].value == -1)
                return false;
            if (IsPartOfHorizMatch(row, col))
                return true;
            return IsPartOfVerticalMatch(row, col);
        }

        bool IsPartOfVerticalMatch(int row, int col)
        {
            var minRow = row - 2 >= 0 ? row - 2 : 0;
            for (int i = minRow; i <= row; i++)
            {
                if (i + 2 >= _grid.rows)
                    return false;
                if (_grid[i, col] == _grid[i + 1, col] && _grid[i, col] == _grid[i + 2, col])
                    return true;
            }

            return false;
        }

        bool IsPartOfHorizMatch(int row, int col)
        {
            var minCol = col - 2 >= 0 ? col - 2 : 0;
            for (int i = minCol; i <= col; i++)
            {
                if (i + 2 >= _grid.columns)
                    return false;
                if (_grid[row, i] == _grid[row, i + 1] && _grid[row, i] == _grid[row, i + 2])
                    return true;
            }

            return false;
        }

        List<TileGridElement> GetMatchedElements(int row, int col)
        {
            var outPut = new List<TileGridElement>();
            outPut = outPut.Concat(GetHorizMatchElements(row, col)).ToList();
            outPut = outPut.Concat(GetVertMatchElements(row, col)).ToList();
            outPut = outPut.Distinct().ToList();
            return outPut;
        }

        List<TileGridElement> GetHorizMatchElements(int row, int col)
        {
            var outPut = new List<TileGridElement>(5);
            var minCol = col - 2 >= 0 ? col - 2 : 0;
            for (int i = minCol; i <= col; i++)
            {
                if (i + 2 >= _grid.columns)
                    return outPut;
                if (_grid[row, i] == _grid[row, i + 1] && _grid[row, i] == _grid[row, i + 2])
                {
                    outPut.Add(_grid[row, i]);
                    outPut.Add(_grid[row, i + 1]);
                    outPut.Add(_grid[row, i + 2]);
                }
            }

            return outPut;
        }

        List<TileGridElement> GetVertMatchElements(int row, int col)
        {
            var outPut = new List<TileGridElement>(5);
            var minRow = row - 2 >= 0 ? row - 2 : 0;
            for (int i = minRow; i <= row; i++)
            {
                if (i + 2 >= _grid.rows)
                    return outPut;
                if (_grid[i, col] == _grid[i + 1, col] && _grid[i, col] == _grid[i + 2, col])
                {
                    outPut.Add(_grid[i, col]);
                    outPut.Add(_grid[i + 1, col]);
                    outPut.Add(_grid[i + 2, col]);
                }
            }

            return outPut;
        }

        public void SetGrid(TilesGrid grid)
        {
            _grid = grid;
        }

        #endregion
    }
}