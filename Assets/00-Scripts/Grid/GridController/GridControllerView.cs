using System.Collections;
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

        private IEnumerator Start()
        {
            yield return null;
            CreateGrid();
        }

        private void OnDestroy()
        {
            _eventController.onDispose.Trigger();
        }

        #endregion

        #region Methods

        [Button]
        void CreateMockGrid()
        {
            _eventController.onCreateMockGridRequest.Trigger();
        }

        [Button]
        void CreateGrid()
        {
            _eventController.onCreateGridRequest.Trigger();
            PrintGrid();
        }

        [Button]
        void PrintGrid()
        {
            var grid = _eventController.onGridRequest.GetFirstResult();
        }

        [Button]
        void Shuffle()
        {
            _eventController.onShuffleRequest.Trigger();
            PrintGrid();
        }

        [Button]
        void DropCheck()
        {
            _eventController.onAfterMatch.Trigger();
        }

        #endregion
    }
}