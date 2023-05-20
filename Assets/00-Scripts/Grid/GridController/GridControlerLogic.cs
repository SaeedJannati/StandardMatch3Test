using System;
using System.Linq;
using Match3.Auxiliary;
using Match3.EventController;
using Newtonsoft.Json;
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
        [Inject] private ElementsDropHandler _dropHandler;
        private TilesGrid _grid;
        private bool _inputEnabled;
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
            _eventController.onInputEnable.Add(OnInputEnable);
            _eventController.onAfterMatch.Add(OnAfterMatch);
            _eventController.onAfterDrop.Add(OnAfterDrop);
            _eventController.onFillEmptySlotsRequest.Add(OnFillEmptySlotsRequest);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onDispose.Remove(Dispose);
            _eventController.onCreateGridRequest.Remove(OnGridCreateRequest);
            _eventController.onGridRequest.Remove(OnGridRequest);
            _eventController.onShuffleRequest.Remove(OnShuffleRequest);
            _eventController.onSwipeRequest.Remove(OnSwipeRequest);
            _eventController.onInputEnable.Remove(OnInputEnable);
            _eventController.onAfterMatch.Remove(OnAfterMatch);
            _eventController.onAfterDrop.Remove(OnAfterDrop);
            _eventController.onFillEmptySlotsRequest.Remove(OnFillEmptySlotsRequest);
        }

        private void OnFillEmptySlotsRequest()
        {
            var emptySlots = _grid.elements.Where(i => i.value == -1);
            foreach (var element in emptySlots)
            {
                SetElementAmount(element.row, element.col);
                _eventController.onElementValueChange.Trigger((element.row,element.col,element.value));
            }
        }

        private void OnAfterDrop()
        {
            _matchChecker.CheckFilledElements();
        }

        private void OnAfterMatch()
        {
            _dropHandler.DropNeededElements();
        }

        private void OnInputEnable(bool enable) => _inputEnabled = enable;
        private void OnSwipeRequest((int row, int col, Direction dir) info)
        {
            if(!_inputEnabled)
                return;
            var element = new Vector2Int(info.row, info.col);
            element += GetDeltaPos(info.dir);
            if (!IsValidElement(element))
            {
                return;
            }

            SwapElements(new Vector2Int(info.row, info.col), element);
            if (!_matchChecker.IsPartOfMatch(info.row, info.col) && !_matchChecker.IsPartOfMatch(element.x, element.y))
            {
                SwapElements(new Vector2Int(info.row, info.col), element);
                return;
            }

            _eventController.onElementValueChange.Trigger((info.row, info.col, _grid[info.row, info.col].value));
            _eventController.onElementValueChange.Trigger((element.x, element.y, _grid[element.x, element.y].value));

            _matchChecker.CheckNeedToCheckElementsForMatch();
        }

        void SwapElements(Vector2Int firstCoords, Vector2Int secondCoords)
        {
            var value = _grid[firstCoords.x, firstCoords.y].value;
            _grid[firstCoords.x, firstCoords.y].SetValue(_grid[secondCoords.x, secondCoords.y].value);
            _grid[secondCoords.x, secondCoords.y].SetValue(value);
        }

        bool IsValidElement(Vector2Int coord)
        {
            if (coord.x < 0)
                return false;
            if (coord.x > _grid.rows - 1)
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
            _eventController.onGridCreated.Trigger();
            _eventController.onInputEnable.Trigger(true);
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