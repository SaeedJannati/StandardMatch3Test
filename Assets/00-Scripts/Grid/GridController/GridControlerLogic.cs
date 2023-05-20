using System;
using System.Collections;
using System.Collections.Generic;
using Match3.EventController;
using UnityEngine;
using Zenject;

namespace Match3.General
{
    public class GridControllerLogic:IEventListener,IDisposable
    {
        #region Fields

        [Inject] private GridControllerEventController _eventController;
        [Inject] private GridControllerView _view;
        [Inject] private GridGenerator _gridGenerator;

        private TilesGrid _gird;
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

        private TilesGrid OnGridRequest() => _gird;

        private void OnGridCreateRequest()
        {
            _gird = _gridGenerator.CreateGrid();
        }

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

