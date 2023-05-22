using System;
using System.Linq;
using System.Threading.Tasks;
using Match3.EventController;
using Match3.General.MoveTest;
using UnityEngine;
using Zenject;

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
        [Inject] private GridMoveEffectsHandler _moveEffects;
        [Inject] private GridMoveEffectsModel _moveEffectsModel;
        [Inject] private GridShuffleController _shuffleController;
        [Inject] private MoveTestEventController _testEventController;
        private TilesGrid _grid;
        private bool _inputEnabled;
//this will only be used in editor window
        public static Action onCreateGridRequest;
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
            _eventController.onShuffleRequest.Add(OnShuffleRequest);
            _eventController.onSwipeRequest.Add(OnSwipeRequest);
            _eventController.onInputEnable.Add(OnInputEnable);
            _eventController.onAfterMatch.Add(OnAfterMatch);
            _eventController.onAfterDrop.Add(OnAfterDrop);
            _eventController.onFillEmptySlotsRequest.Add(OnFillEmptySlotsRequest);
            _eventController.onCreateMockGridRequest.Add(OnCreateMockGridRequest);
            _eventController.onAfterShuffle.Add(OnAfterShuffle);
            _eventController.onUpdateTilesColours.Add(OnUpdateTilesColours);
            onCreateGridRequest += OnGridCreateRequest;
        }

        public void UnregisterFromEvents()
        {
            _eventController.onDispose.Remove(Dispose);
            _eventController.onCreateGridRequest.Remove(OnGridCreateRequest);
            _eventController.onShuffleRequest.Remove(OnShuffleRequest);
            _eventController.onSwipeRequest.Remove(OnSwipeRequest);
            _eventController.onInputEnable.Remove(OnInputEnable);
            _eventController.onAfterMatch.Remove(OnAfterMatch);
            _eventController.onAfterDrop.Remove(OnAfterDrop);
            _eventController.onFillEmptySlotsRequest.Remove(OnFillEmptySlotsRequest);
            _eventController.onCreateMockGridRequest.Remove(OnCreateMockGridRequest);
            _eventController.onAfterShuffle.Remove(OnAfterShuffle);
            _eventController.onUpdateTilesColours.Remove(OnUpdateTilesColours);
            onCreateGridRequest -= OnGridCreateRequest;
        }

        private void OnUpdateTilesColours()
        {
            foreach (var element in _grid.elements)
            {
                _eventController.onElementValueChange.Trigger((element.row, element.col, element.value));
            }
        }

        private void OnAfterShuffle()
        {
            _matchChecker.CheckFilledElements();
        }

        private async void OnFillEmptySlotsRequest()
        {
            var emptySlots = _grid.elements.Where(i => i.value == -1).ToList();
            if (emptySlots.Count == 0)
            {
                _shuffleController.OnShuffleNeedCheck();
                return;
            }

            var rows = _grid.rows;
            emptySlots.Sort((x, y) => (x.col * rows + x.row).CompareTo(y.col * rows + y.row));
            var depthInCol = 0;
            var lastCol = -1;
            var isPartOfMatch = true;
            for (int i = emptySlots.Count - 1; i >= 0; i--)
            {
                if (emptySlots[i].col != lastCol)
                {
                    lastCol = emptySlots[i].col;
                    depthInCol = 0;
                }


                _gridGenerator.SetElementAmount(emptySlots[i].row, emptySlots[i].col);


                _moveEffects.ApplySpawnEffect(emptySlots[i], depthInCol);
                depthInCol++;
            }

            await CheckForDelay();
            if (_shuffleController.OnShuffleNeedCheck())
                return;
            _testEventController.onNextMovePossible.Trigger();
        }

        async Task CheckForDelay()
        {
            if (IsNonGraphicalTest())
                return;
            _eventController.onInputEnable.Trigger(false);
            await Task.Delay((int)(1000 * _moveEffectsModel.spawnPeriod));
            _eventController.onInputEnable.Trigger(true);
        }

        bool IsNonGraphicalTest()
        {
            return _testEventController.onNonGraphicalTestRunningRequest.GetFirstResult();
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

        private async void OnSwipeRequest((int row, int col, Direction dir) info)
        {
            if (!_inputEnabled)
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

            if (!IsNonGraphicalTest())
                await ApplySwipeViewEffect(_grid[info.row, info.col], _grid[element.x, element.y]);
            _matchChecker.CheckNeedToCheckElementsForMatch();
        }

        async Task ApplySwipeViewEffect(TileGridElement first, TileGridElement second)
        {
            await _moveEffects.SwapElements(first, second);

            _eventController.onInputEnable.Trigger(false);
            await Task.Delay((int)(1000 * _moveEffectsModel.shuffleDelayPeriod));
            _eventController.onInputEnable.Trigger(true);
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
            _shuffleController.ShuffleGrid();
        }

        private  void OnGridCreateRequest()
        {
            _grid =  _gridGenerator.CreateGrid();
            if (_shuffleController.OnShuffleNeedCheck())
                return;
            _testEventController.onNextMovePossible.Trigger();
        }

        private void OnCreateMockGridRequest()
        {
            _grid = _gridGenerator.CreateMockGrid();
        }


        public void Dispose()
        {
            UnregisterFromEvents();
            _gridGenerator.Dispose();
            _matchChecker.Dispose();
            _moveEffects.Dispose();
            _shuffleController.Dispose();
            _eventController.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}