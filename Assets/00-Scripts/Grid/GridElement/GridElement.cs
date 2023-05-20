using Match3.Auxiliary;
using UnityEngine;
using Zenject;

namespace Match3.General
{
    public class GridElement : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Swipeable _swipeable;
        [SerializeField] private SpriteRenderer _renderer;

        [Inject] private GridControllerEventController _gridEventController;
        public int row;
        public int col;

        #endregion

        #region Unity actions

        private void Start()
        {
            _swipeable.onSwipe += OnSwipe;
        }

        private void OnDestroy()
        {
            _swipeable.onSwipe -= OnSwipe;
        }

        #endregion

        #region Methods

        [Inject]
        void Construct(int aRow, int aCol)
        {
            row = aRow;
            col = aCol;
        }

        public void SetColour(Color colour)
        {
            _renderer.color = colour;
        }

        private void OnSwipe(Direction direction)
        {
            _gridEventController.onSwipeRequest.Trigger((row, col, direction));
        }

        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<int, int, GridControllerEventController, GridElement>
        {
        }

        #endregion
    }
}