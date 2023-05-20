using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace  Match3.General
{
    public class GridGenerator:IDisposable
    {
        #region Fields

        [Inject] private GridGeneratorModel _model;


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

