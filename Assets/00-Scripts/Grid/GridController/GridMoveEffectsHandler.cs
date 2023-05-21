using System;
using System.Threading.Tasks;
using DG.Tweening;
using Match3.EventController;
using UnityEngine;
using Zenject;

namespace Match3.General
{
    public class GridMoveEffectsHandler:IDisposable,IInitializable,IEventListener
    {
        #region Fields

        [Inject] private GridMoveEffectsModel _model;
        [Inject] private GridControllerEventController _eventController;
        private TilesGrid _grid;

        #endregion

        #region Methods

        public void Dispose()
        {
            UnregisterFromEvents();
            GC.SuppressFinalize(this);
        }

        public void Initialize()
        {
            RegisterToEvents();
        }
        public void RegisterToEvents()
        {
            _eventController.onGridCreated.Add(OnGridCreated);
            _eventController.onTileViewFadeRequest.Add(OnTileViewFadeRequest);
        }
        public void UnregisterFromEvents()
        {
            _eventController.onGridCreated.Remove(OnGridCreated);
            _eventController.onTileViewFadeRequest.Remove(OnTileViewFadeRequest);
        }

        private async void OnTileViewFadeRequest(TileGridElement element)
        {
            var period = _model.tileFadePeriod;
            var view = _eventController.onRequestTileView.GetFirstNonDefaultResult(element);
            view.spriteRenderer.DOColor(Color.black, period);
        }

        private void OnGridCreated()
        {
            _grid = _eventController.onGridRequest.GetFirstResult();
        }

       public async Task SwapElements(TileGridElement firstElement,TileGridElement secondElement)
        {
            var first = _eventController.onRequestTileView.GetFirstNonDefaultResult(firstElement).transform;
            var second = _eventController.onRequestTileView.GetFirstNonDefaultResult(secondElement).transform;
            var firstInitPos = first.position;
            var secondInitPos = second.position;
            var period = _model.tileSwipePeriod;
            first.DOMove(secondInitPos, period);
            second.DOMove(firstInitPos, period);
            await Task.Delay((int)(1000 * period));
            first.position = firstInitPos;
            second.position = secondInitPos;
        }

        #endregion



    }
}