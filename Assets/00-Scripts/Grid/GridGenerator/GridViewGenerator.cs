using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Match3.General
{
    public class GridViewGenerator:MonoBehaviour
    {
        #region Fields

        [Inject] private GridElement.Factory _elementFactory;
        [Inject] private GridGeneratorViewModel _model;
        [Inject] private GridControllerEventController _gridEventController;
        [SerializeField] private int colourIndex;

        #endregion

        #region Methods

        [Button]
        void CreateSampleElement()
        {
            var element = _elementFactory.Create(0, 1, _gridEventController);
            var col = _model.colours[colourIndex];
            element.SetColour(col);
        }

        #endregion
    }
}