using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Match3.Auxiliary;
using Match3.EventController;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Match3.General
{
    public class GridControllerLogic : IEventListener, IDisposable
    {
        #region Fields

        [Inject] private GridControllerEventController _eventController;
        [Inject] private GridControllerView _view;
        [Inject] private GridGenerator _gridGenerator;

        private TilesGrid _grid;

        #endregion

        #region Methods

        [Inject]
        void Initialise()
        {
            RegisterToEvents();
        }

        public void RegisterToEvents()
        {
            _eventController.onDispose.Add(Dispose);
            _eventController.onCreateGridRequest.Add(OnGridCreateRequest);
            _eventController.onGridRequest.Add(OnGridRequest);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onDispose.Remove(Dispose);
            _eventController.onCreateGridRequest.Remove(OnGridCreateRequest);
            _eventController.onGridRequest.Remove(OnGridRequest);
        }

        private TilesGrid OnGridRequest() => _grid;

        private void OnGridCreateRequest()
        {
            _grid = _gridGenerator.CreateGrid();
            FillTheGrid();
        }

        async void FillTheGrid()
        {
            var count = _grid.count;
            for (int i = 0; i < _grid.rows; i++)
            {
                for (int j = 0; j < _grid.columns; j++)
                {
                    await SetElementAmount(i, j);
                }
            }
        }

        async Task SetElementAmount(int row, int col)
        {
            var amount = GetRandomAmount();
            _grid[row,col].SetValue(amount,false);
            await Task.Delay(1);
            if (IsPartOfMatch(row, col))
                SetElementAmount(row, col);
        }

        bool IsPartOfMatch(int row, int col)
        {
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

        int GetRandomAmount() => Random.Range(0, _gridGenerator.colourCount);


        public void Dispose()
        {
            UnregisterFromEvents();
            _gridGenerator?.Dispose();
            _eventController.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}