using System;
using System.Threading.Tasks;
using DG.Tweening;
using Match3.EventController;
using Match3.General.MoveTest;
using UnityEngine;
using Zenject;

namespace Match3.General
{
    public class GridMoveEffectsHandler : IDisposable, IInitializable, IEventListener
    {
        #region Fields

        [Inject] private GridMoveEffectsModel _model;
        [Inject] private GridControllerEventController _eventController;
        [Inject] private MoveTestEventController _testEventController;
        private TilesGrid _grid;

        #endregion

        #region Methods

       

        public void Initialize()
        {
            RegisterToEvents();
        }

        public void RegisterToEvents()
        {
            _eventController.onGridCreated.Add(OnGridCreated);
            _eventController.onTileViewFadeRequest.Add(OnTileViewFadeRequest);
            _eventController.onDropEffectRequest.Add(OnDropEffectRequest);
            _eventController.onShuffleEffectRequest.Add(OnShuffleEffectRequest);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onGridCreated.Remove(OnGridCreated);
            _eventController.onTileViewFadeRequest.Remove(OnTileViewFadeRequest);
            _eventController.onDropEffectRequest.Remove(OnDropEffectRequest);
            _eventController.onShuffleEffectRequest.Remove(OnShuffleEffectRequest);
        }

        private async void OnShuffleEffectRequest()
        {
            await ShuffleGraphicalEffect();
            _eventController.onAfterShuffle.Trigger();
        }

        async Task ShuffleGraphicalEffect()
        {
            if(IsNonGraphicalTest())
                return;
            _eventController.onInputEnable.Trigger(false);
            var fadePeriod = _model.shuffleFadePeriod;
            _eventController.onFadeGridRequest.Trigger((true, fadePeriod));
            await Task.Delay((int)(1000 * fadePeriod));
            var delay = _model.shuffleDelayPeriod;
            await Task.Delay((int)(1000 * delay+100));
            foreach (TileGridElement element in _grid.elements)
            {
                _eventController.onElementValueChange.Trigger((element.row, element.col, element.value));
            }
            _eventController.onFadeGridRequest.Trigger((false, fadePeriod));
            await Task.Delay((int)(1000 * fadePeriod));
            _eventController.onInputEnable.Trigger(true);
        }

        async void OnDropEffectRequest((TileGridElement elemnetToDrop, TileGridElement destElement) info)
        {
            if(IsNonGraphicalTest())
                return;
            var period = _model.dropPeriod;
            var view = _eventController.onRequestTileView.GetFirstNonDefaultResult(info.elemnetToDrop);
            var dest = _eventController.onRequestTileView.GetFirstNonDefaultResult(info.destElement);
            var initPos = view.transform.position;
            var destPos = dest.transform.position;
            view.spriteRenderer.sortingOrder++;
            view.transform.DOMove(destPos, period);
            await Task.Delay((int)(1000 * period));
            view.spriteRenderer.sortingOrder--;
            view.transform.position = initPos;
            _eventController.onElementValueChange.Trigger((info.elemnetToDrop.row, info.elemnetToDrop.col,
                info.elemnetToDrop.value));
            _eventController.onElementValueChange.Trigger((info.destElement.row, info.destElement.col,
                info.destElement.value));
        }

        private async void OnTileViewFadeRequest(TileGridElement element)
        {
            if(IsNonGraphicalTest())
                return;
            var period = _model.tileFadePeriod;
            var view = _eventController.onRequestTileView.GetFirstNonDefaultResult(element);
            view.spriteRenderer.DOColor(Color.black, period);
            await Task.Delay((int)(1000 * period));
            if (element.value == -1)
                _eventController.onElementValueChange.Trigger((element.row, element.col, element.value));
        }

        private void OnGridCreated()
        {
            _grid = _eventController.onGridRequest.GetFirstResult();
        }

        public async Task SwapElements(TileGridElement firstElement, TileGridElement secondElement)
        {
            await SwapElementsGraphicalEffects(firstElement,secondElement);
            _eventController.onElementValueChange.Trigger((firstElement.row, firstElement.col, firstElement.value));
            _eventController.onElementValueChange.Trigger((secondElement.row, secondElement.col, secondElement.value));
            await Task.Yield();
        }

        public async Task SwapElementsGraphicalEffects(TileGridElement firstElement, TileGridElement secondElement)
        {
            if(IsNonGraphicalTest())
                return;
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

        public async Task GridCreationEffect()
        {
            _eventController.onInputEnable.Trigger(false);
            var fadePeriod = _model.shuffleFadePeriod;
            _eventController.onFadeGridRequest.Trigger((false, fadePeriod));
            await Task.Delay((int)(1000 * fadePeriod));
            _eventController.onInputEnable.Trigger(true);
        }

        public async  void ApplySpawnEffect(TileGridElement element,int depthInCol)
        {

            if(IsNonGraphicalTest())
                return;
            _eventController.onElementValueChange.Trigger((element.row, element.col, element.value));
            var period = _model.spawnPeriod;
            var view = _eventController.onRequestTileView.GetFirstNonDefaultResult(element);
            var initPos = view.transform.position;
            var destPos = initPos;
            destPos.y += element.row + depthInCol + 1;
            view.transform.position = destPos;
            view.transform.DOMove(initPos, period).SetEase(Ease.Linear);
            await Task.Delay((int)(1000 * period));
            view.transform.position = initPos;
        }
        public void Dispose()
        {
            UnregisterFromEvents();
            GC.SuppressFinalize(this);
        }

        bool IsNonGraphicalTest()
        {
            return _testEventController.onNonGraphicalTestRunningRequest.GetFirstResult();
        }

        #endregion


    }
}