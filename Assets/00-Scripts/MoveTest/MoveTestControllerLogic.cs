using System;
using System.Threading.Tasks;
using Match3.Auxiliary;
using Match3.EventController;
using UnityEngine;
using Zenject;

namespace Match3.General.MoveTest
{
    public class MoveTestControllerLogic : IDisposable, IInitializable, IEventListener
    {
        #region Fields

        [Inject] private GridControllerEventController _gridEventController;
        [Inject] private MoveTestEventController _eventController;
        [Inject] private MoveTestControllerModel _model;

        private bool _isNextMovePossible;
        private bool _isTestEnable;
        private int _remainingMoves;
        private float time;
        #endregion

        #region Methods

        public void Dispose()
        {
            UnregisterFromEvents();
            _eventController.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Initialize()
        {
            RegisterToEvents();
        }

        public void RegisterToEvents()
        {
            _eventController.onDispose.Add(Dispose);
            _eventController.onTestEnable.Add(OnTestEnable);
            _eventController.onNextMovePossible.Add(OnNextMovePossible);
            _eventController.onTestBegin.Add(OnTestBegin);
            _eventController.onNonGraphicalTestRunningRequest.Add(OnNonGraphicalTestRunningRequest);
            _eventController.onTestFinish.Add(OnTestFinish);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onDispose.Remove(Dispose);
            _eventController.onTestEnable.Remove(OnTestEnable);
            _eventController.onNextMovePossible.Remove(OnNextMovePossible);
            _eventController.onTestBegin.Remove(OnTestBegin);
            _eventController.onNonGraphicalTestRunningRequest.Remove(OnNonGraphicalTestRunningRequest);
            _eventController.onTestFinish.Remove(OnTestFinish);
            
        }

        private void OnTestFinish()
        {
            time = Time.time-time;
            GameLogger.Log($"End|Duration:{time}");
            _gridEventController.onUpdateTilesColours.Trigger();
        }

        private bool OnNonGraphicalTestRunningRequest()
        {
            if (!_isTestEnable)
                return false;
            return !_model.isGraphicalTest;
        }


        private void OnTestBegin()
        {
            _remainingMoves = _model.testMoveCount;
            OnNextMovePossible();
            time = Time.time;
            GameLogger.Log($"Begin|MoveCount:{_model.testMoveCount}");
        }

        private async void OnNextMovePossible()
        {
            if (!_isTestEnable)
                return;
            if(CheckForTestEnd())
                return;
            await Task.Delay(1);
            _gridEventController.onRandomMoveRequest.Trigger();
            _remainingMoves--;
        }

        bool CheckForTestEnd()
        {
            if (_remainingMoves > 0)
                return false;
            _eventController.onTestFinish.Trigger();
            _eventController.onTestEnable.Trigger(false);
            return true;
        }

        private void OnTestEnable(bool enable)
        { 
            _isTestEnable = enable;
        } 

        #endregion
    }
}