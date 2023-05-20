using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Auxiliary;
using Match3.EventController;
using TMPro;
using Zenject;

namespace Match3.General
{
    public class MatchChecker : IDisposable, IEventListener
    {
        #region Fields

        [Inject] private GridControllerEventController _gridEventController;
        [Inject] private MatchCheckerEventController _eventController;
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
        }

        public void UnregisterFromEvents()
        {
        }

        public void CheckNeedToCheckElementsForMatch()
        {
            var elementsToCheck = _grid.elements.Where(i => i.needCheck).ToList();
            CheckElementsForMatch(elementsToCheck);
        }

        bool CheckElementsForMatch(List<TileGridElement> elements)
        {
            var matched = false;
            foreach (var element in elements)
            {
                matched |= CheckMatchForElement(element);
            }

            if (!matched)
                return false;
            _gridEventController.onAfterMatch.Trigger();
            return true;
        }

        public void CheckFilledElements()
        {
            var elementsToCheck = _grid.elements.Where(i => i.value != -1).ToList();
            if (CheckElementsForMatch(elementsToCheck))
            {
                return;
            }
            _gridEventController.onFillEmptySlotsRequest.Trigger();
        }

        bool CheckMatchForElement(TileGridElement element)
        {
            if (!IsPartOfMatch(element.row, element.col))
                return false;
            var elementsToFade =
                GetMatchedElements(element.row, element.col);
            foreach (var aElement in elementsToFade)
            {
                aElement.SetValue(-1, false);
                _gridEventController.onElementValueChange.Trigger((aElement.row, aElement.col, aElement.value));
            }

            return true;
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