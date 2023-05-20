using System;
using Zenject;

namespace  Match3.General
{
    public class GridGenerator:IDisposable
    {
        #region Fields

        [Inject] private GridGeneratorModel _model;


        #endregion

        #region Properties

        public int colourCount => _model.colourCount;
        

        #endregion
        #region Methods
        
        public TilesGrid CreateGrid()
        {
            var grid = new TilesGrid(_model.rowCount, _model.coloumnCount);
            return grid;
        }

        public void Dispose()
        {
        }
        #endregion

        
    }
}

