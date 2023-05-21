
using System;
using Match3.Auxiliary;
using Match3.EventController;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Match3.General.MoveTest
{
    public class MoveTestControllerView : MonoBehaviour,IEventListener
    {
        #region Fields

        [Inject] private MoveTestEventController _eventController;
        //just for the sake of showing it in the inspector so user can tweak test params
        [SerializeField, Expandable] private MoveTestControllerModel _model;  

        #endregion

        #region Unity actions

        private void Start()
        {
            RegisterToEvents();
        }

        private void OnDestroy()
        {
            _eventController.onDispose.Trigger();
            UnregisterFromEvents();
        }

        #endregion

        #region Methods

        [Sirenix.OdinInspector.Button]
        public void RunTheTest()
        {
            if (!Application.isPlaying)
            {
                GameLogger.Log("You need to be in play mode in order to run the test!", GameLogger.Colours.lightRed);
                return;
            }

            _eventController.onTestEnable.Trigger(true);
            _eventController.onTestBegin.Trigger();
        }

        public void RegisterToEvents()
        {
 
        }

        public void UnregisterFromEvents()
        {
    
        }
        #endregion

   
    }
}
