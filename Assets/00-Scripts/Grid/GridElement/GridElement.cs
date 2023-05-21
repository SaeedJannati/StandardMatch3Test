using Match3.Auxiliary;
using Match3.EventController;
using UnityEngine;
using Zenject;

namespace Match3.General
{
    public class GridElement : MonoBehaviour, IEventListener
    {
        #region Fields

        [SerializeField] private Swipeable _swipeable;
        [field: SerializeField] public SpriteRenderer spriteRenderer { get; private set; }

        [Inject] private GridControllerEventController _gridEventController;
        [Inject] private GridGeneratorViewModel _model;
        public int row;
        public int col;

        #endregion

        #region Unity actions

        private void Start()
        {
            _swipeable.onSwipe += OnSwipe;
            RegisterToEvents();
        }

        private void OnDestroy()
        {
            _swipeable.onSwipe -= OnSwipe;
            UnregisterFromEvents();
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
            spriteRenderer.color = colour;
        }

        private void OnSwipe(Direction direction)
        {
            _gridEventController.onSwipeRequest.Trigger((row, col, direction));
        }

        public void RegisterToEvents()
        {
            _gridEventController.onElementValueChange.Add(OnElementValueChange);
            _gridEventController.onRequestTileView.Add(OnRequestTileView);
        }

        public void UnregisterFromEvents()
        {
            _gridEventController.onElementValueChange.Remove(OnElementValueChange);
            _gridEventController.onRequestTileView.Remove(OnRequestTileView);
        }

        private GridElement OnRequestTileView(TileGridElement element)
        {
            if (element.row != row)
                return default;
            if (element.col != col)
                return default;
            return this;
        }

        private void OnElementValueChange((int row, int col, int value) info)
        {
            if (info.col != col)
                return;
            if (info.row != row)
                return;
            var colour = Color.black;
            if (info.value >= 0)
                colour =_model.colours[info.value];

            SetColour(colour);
        }

        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<int, int, GridControllerEventController, GridGeneratorViewModel,
            GridElement>
        {
        }

        #endregion
    }
}