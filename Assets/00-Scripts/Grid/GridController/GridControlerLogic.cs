using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        [Inject] private MatchChecker _matchChecker;
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
            _eventController.onShuffleRequest.Add(OnShuffleRequest);
            _eventController.onSwipeRequest.Add(OnSwipeRequest);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onDispose.Remove(Dispose);
            _eventController.onCreateGridRequest.Remove(OnGridCreateRequest);
            _eventController.onGridRequest.Remove(OnGridRequest);
            _eventController.onShuffleRequest.Remove(OnShuffleRequest);
            _eventController.onSwipeRequest.Remove(OnSwipeRequest);
        }

        private void OnSwipeRequest((int row, int col, Direction dir) info)
        {
            var element = new Vector2Int(info.row, info.col);
            element += GetDeltaPos(info.dir);
            if (!IsValidElement(element))
            {
                return;
            }

            var value = _grid[info.row, info.col].value;
            _grid[info.row, info.col].SetValue(_grid[element.x,element.y].value);
            _grid[element.x,element.y].SetValue(value);
            _eventController.onElementValueChange.Trigger((info.row, info.col, _grid[info.row, info.col].value));
            _eventController.onElementValueChange.Trigger((element.x, element.y, _grid[element.x,  element.y].value));
            
          _matchChecker.CheckNeedToCheckElementsForMatch();
        }

        bool IsValidElement(Vector2Int coord)
        {
            if (coord.x < 0)
                return false;
            if (coord.x > _grid.rows-1)
                return false;
            if (coord.y < 0)
                return false;
            if (coord.y > _grid.columns - 1)
                return false;
            return true;
        }

        Vector2Int GetDeltaPos(Direction dir)
        {
            return dir switch
            {
                Direction.Left => new Vector2Int(0, -1),
                Direction.Right => new Vector2Int(0, 1),
                Direction.Up => new Vector2Int(-1, 0),
                Direction.Down => new Vector2Int(1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }

        private void OnShuffleRequest()
        {
            ShuffleGrid();
        }

        private TilesGrid OnGridRequest() => _grid;

        private void OnGridCreateRequest()
        {
            _grid = _gridGenerator.CreateGrid();
            _matchChecker.SetGrid(_grid);
            FillTheGrid();
            _eventController.onGridCreated?.Trigger();
        }

        void FillTheGrid()
        {
            var count = _grid.count;
            for (int i = 0; i < _grid.rows; i++)
            {
                for (int j = 0; j < _grid.columns; j++)
                {
                    SetElementAmount(i, j);
                }
            }
        }

        void SetElementAmount(int row, int col)
        {
            var amount = GetRandomAmount();
            _grid[row, col].SetValue(amount, false);
            if (_matchChecker.IsPartOfMatch(row, col))
                SetElementAmount(row, col);
        }

     

        int GetRandomAmount() => Random.Range(0, _gridGenerator.colourCount);


        public void Dispose()
        {
            UnregisterFromEvents();
            _gridGenerator.Dispose();
            _eventController.Dispose();
            _matchChecker.Dispose();
            GC.SuppressFinalize(this);
        }

        void ShuffleGrid()
        {
            var values = _grid.elements.Select(i => i.value).ToList();
            values.Shuffle();
            for (int i = 0, e = _grid.count; i < e; i++)
            {
                _grid[i].SetValue(values[i], false);
            }
        }

        #endregion
    }
}