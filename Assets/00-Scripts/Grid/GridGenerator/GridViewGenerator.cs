using System;
using Match3.Auxiliary;
using Match3.EventController;
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

        #region Unity Actions

        private void Start()
        {
            RegisterToEvents();
        }

        private void OnDestroy()
        {
            UnregisterFromEvents();
        }

        #endregion

        #region Methods

        [Button]
        void CreateSampleElement()
        {
            CreateGridElement(0, 1,colourIndex);
        }

        void CreateGridElement(int row, int col,int value)
        {
            var element = _elementFactory.Create(row, col, _gridEventController);
            var colour = _model.colours[value];
            element.SetColour(colour);
            element.transform.SetParent(transform);
            element.transform.localPosition = new Vector3(col, row, 0);
        }

        void RegisterToEvents()
        {
            _gridEventController.onGridCreated.Add(OnGridCreated);
        }

        void UnregisterFromEvents()
        {
            _gridEventController.onGridCreated.Remove(OnGridCreated);
        }

        private void OnGridCreated()
        {
            transform.ClearChildren();
            CreateGridElements();
        }

        private void CreateGridElements()
        {
            var grid = _gridEventController.onGridRequest.GetFirstResult();
            var rows = grid.rows;
            var cols = grid.columns;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    CreateGridElement(i,j,grid[i,j].value);
                }
            }

            var pos = new Vector3(-cols / 2.0f,-rows/2.0f);
            transform.position = pos;
        }

        #endregion
    }
}