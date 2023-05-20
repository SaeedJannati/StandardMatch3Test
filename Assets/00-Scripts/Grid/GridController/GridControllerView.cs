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
        }

        [Button]
        void PrintGrid()
        {
            var grid=_eventController.onGridRequest.GetFirstResult();
            GameLogger.Log(grid.ToString(),GameLogger.Colours.beige);
        }

        #endregion
    }
}
