using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Match3.Auxiliary;
using Match3.EventController;
using Zenject;

namespace Match3.General
{
    public class GridGenerator : IDisposable, IEventListener
    {
        #region Fields

        [Inject] private GridControllerEventController _eventController;
        [Inject] private GridGeneratorModel _model;
        [Inject] private MatchChecker _matchChecker;
        private TilesGrid _grid;
        private List<int> possibleColours;
        #endregion

        #region Properties

        public int colourCount => _model.colourCount;

        #endregion

        #region Methods

        [Inject]
        void Initialise()
        {
            RegisterToEvents();
            FillThePossibleValues();
        }

        public  TilesGrid CreateGrid()
        {
            _grid = new TilesGrid(_model.rowCount, _model.coloumnCount);
            _matchChecker.SetGrid(_grid);
             FillTheGrid();
            _eventController.onGridCreated.Trigger();
            _eventController.onInputEnable.Trigger(true);
            return _grid;
        }

        public TilesGrid CreateMockGrid()
        {
            var grid = new TilesGrid(_model.rowCount, _model.coloumnCount);
            _matchChecker.SetGrid(_grid);
            FillTheGridWithMockData();
            _eventController.onGridCreated.Trigger();
            _eventController.onInputEnable.Trigger(true);
            return _grid;
        }

         void FillTheGrid()
        {
            for (int i = 0; i < _grid.rows; i++)
            {
                for (int j = 0; j < _grid.columns; j++)
                {
                    SetElementAmount(i, j);
                }
            }
        }

         void FillThePossibleValues()
         {
             possibleColours=Enumerable.Range(0, _model.colourCount).ToList();
             
         }

         public void SetElementAmount(int row, int col)
         {
             possibleColours.Shuffle();
             foreach (var amount in possibleColours)
             {
                 _grid[row, col].SetValue(amount, false);
                 if (!_matchChecker.IsPartOfMatch(row, col))
                     return;
             }
            
        }


        void FillTheGridWithMockData()
        {
            _model.ExtractMockGridValues();
            var amounts = _model.mockGridElementsValue;
            for (int i = 0, e = amounts.Count; i < e; i++)
            {
                _grid[i].SetValue(amounts[i], false);
            }
        }

        int GetRandomAmount() => UnityEngine.Random.Range(0, _model.colourCount);

        public void Dispose()
        {
            UnregisterFromEvents();
            GC.SuppressFinalize(this);
        }

        public void RegisterToEvents()
        {
            _eventController.onGridRequest.Add(OnGridRequest);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onGridRequest.Remove(OnGridRequest);
        }

        private TilesGrid OnGridRequest() => _grid;

        #endregion
    }
}