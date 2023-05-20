using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Auxiliary;
using Match3.EventController;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Match3.General
{
    public class ElementsDropHandler : IDisposable, IEventListener, IInitializable
    {
        #region Fields

        [Inject] private GridControllerEventController _gridEventController;
        private TilesGrid _grid;

        #endregion

        #region Methods

        public void Initialize()
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
            _gridEventController.onGridCreated.Add(OnGridCreated);
        }

        public void UnregisterFromEvents()
        {
            _gridEventController.onGridCreated.Remove(OnGridCreated);
        }

        private void OnGridCreated()
        {
            GetGrid();
        }

        void GetGrid()
        {
            _grid = _gridEventController.onGridRequest.GetFirstResult();
        }

        public void DropNeededElements()
        {
            var colsWithEmptyTile = GetColsWithEmptyTile();
            if (colsWithEmptyTile.Count == 0)
                return;
            foreach (var col in colsWithEmptyTile)
            {
                DropElementsInCol(col);
            }
        }


        List<int> GetColsWithEmptyTile()
        {
            var cols = _grid.elements.Where(i => i.value == -1)
                .Select(j => j.col).Distinct()
                .ToList();
            return cols;
        }

        void DropElementsInCol(int col)
        {
            var filledElements = GetFilledElementsInCol(col);
            var emptyElements = GetEmptyElementsInCol(col);
            var maxEmptyRow = emptyElements.Max(i => i.row);
            var minEmptyRow = emptyElements.Min(i => i.row);
            filledElements = filledElements.Where(i => i.row < minEmptyRow).ToList();
            if (filledElements.Count == 0)
                return;
            var maxFilledRow = filledElements.Max(i => i.row);
            var deltaRow = maxEmptyRow - maxFilledRow;
            var firstCoord = new Vector2Int();
            var secondCoord = new Vector2Int();
            for (int i = filledElements.Count - 1; i >= 0; i--)
            {
                firstCoord.x = filledElements[i].row;
                firstCoord.y = filledElements[i].col;
                secondCoord.x = filledElements[i].row+deltaRow;
                secondCoord.y = filledElements[i].col;
               SwapElements(firstCoord, secondCoord);
                _gridEventController.onElementValueChange.Trigger((firstCoord.x, firstCoord.y,
                    _grid[firstCoord.x, firstCoord.y].value));
                _gridEventController.onElementValueChange.Trigger((secondCoord.x, secondCoord.y,
                    _grid[secondCoord.x, secondCoord.y].value));
            }
        }

        List<TileGridElement> GetEmptyElementsInCol(int col)
        {
            var outPut = _grid.elements
                .Where(i => i.col == col).Where(j => j.value == -1).ToList();
            return outPut;
        }

        List<TileGridElement> GetFilledElementsInCol(int col)
        {
            var outPut = _grid.elements
                .Where(i => i.col == col).Where(j => j.value != -1).ToList();
            return outPut;
        }

        void SwapElements(Vector2Int firstCoords, Vector2Int secondCoords)
        {
            var value = _grid[firstCoords.x, firstCoords.y].value;
            _grid[firstCoords.x, firstCoords.y].SetValue(_grid[secondCoords.x, secondCoords.y].value);
            _grid[secondCoords.x, secondCoords.y].SetValue(value);
        }

        #endregion
    }
}