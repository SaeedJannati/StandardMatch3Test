using System.Threading.Tasks;
using DG.Tweening;
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
        [Inject] private GridGeneratorModel _generatorModel;
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
            _gridEventController.onFadeGridRequest.Add(OnFadeGridRequest);
        }

        public void UnregisterFromEvents()
        {
            _gridEventController.onElementValueChange.Remove(OnElementValueChange);
            _gridEventController.onRequestTileView.Remove(OnRequestTileView);
            _gridEventController.onFadeGridRequest.Remove(OnFadeGridRequest);
        }

        private async void OnFadeGridRequest((bool fade,float period)info)
        {
            var randomDelay = Random.Range(0, 100);
            var initAlpha = info.fade ? 1.0f : 0.0f;
            var destAlpha =info.fade ? 0.0f : 1.0f;
            var initColour = spriteRenderer.color;
            initColour.a = initAlpha;
            spriteRenderer.color = initColour;
            await Task.Delay(randomDelay);
            spriteRenderer.DOFade(destAlpha, info.period);
            transform.DOScale(destAlpha, info.period);
            await Task.Delay((int)(1000 * info.period));
            initColour.a = destAlpha;
            spriteRenderer.color = initColour;
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
                colour =_generatorModel.colours[info.value];
            SetColour(colour);
        }

        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<int, int, GridControllerEventController, GridGeneratorModel,
            GridElement>
        {
        }

        #endregion
    }
}