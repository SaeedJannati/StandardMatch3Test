using System;
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
        }

        public void UnregisterFromEvents()
        {
            _eventController.onDispose.Remove(Dispose);
            _eventController.onTestEnable.Remove(OnTestEnable);
            _eventController.onNextMovePossible.Remove(OnNextMovePossible);
            _eventController.onTestBegin.Remove(OnTestBegin);
        }


        private void OnTestBegin()
        {
            _remainingMoves = _model.testMoveCount;
            OnNextMovePossible();
        }

        private void OnNextMovePossible()
        {
            if (!_isTestEnable)
                return;
            if(CheckForTestEnd())
                return;
            _gridEventController.onRandomMoveRequest.Trigger();
            GameLogger.Log($"Remaining Moves:{_remainingMoves}");
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