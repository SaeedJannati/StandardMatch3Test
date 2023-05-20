using System;
using Match3.Auxiliary;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Match3.General
{
    public class GridControllerView : MonoBehaviour
    {
        #region Fields
        [Inject] private GridControllerEventController _eventController;

        #endregion

        #region Unity actions

        private void Start()
        {
            CreateGrid();
        }

        private void OnDestroy()
        {
            _eventController.onDispose.Trigger();
        }

        #endregion

        #region Methods

        [Button]
        void CreateGrid()
        {
            _eventController.onCreateGridRequest.Trigger();
            PrintGrid();
        }

        [Button]
        void PrintGrid()
        {
            var grid=_eventController.onGridRequest.GetFirstResult();
            GameLogger.Log(grid.ToString());
        }

        [Button]
        void Shuffle()
        {
            _eventController.onShuffleRequest.Trigger();
            PrintGrid();
        }

        #endregion
    }
}
