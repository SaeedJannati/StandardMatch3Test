using System;
using System.Threading.Tasks;
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
        [Inject] private GridGeneratorModel _model;
        [Inject] private GridControllerEventController _gridEventController;
        [SerializeField] private int colourIndex;
        [SerializeField] private SpriteRenderer _gridBorder;
        [SerializeField] private SpriteRenderer _gridBack;
        [SerializeField] private Transform _gridMask;

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
            var element = _elementFactory.Create(row, col,_gridEventController,_model);
            var colour = _model.colours[value];
            element.SetColour(colour);
            element.transform.SetParent(transform);
            element.transform.localPosition = new Vector3(col, -row, 0);
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

            var pos = new Vector3(-(cols-1) / 2.0f,(rows-1)/2.0f);
            transform.position = pos;
            UpdateBackgroundParts(rows,cols);
        }
        private void UpdateBackgroundParts(int rows,int cols)
        {
            _gridMask.localScale = new Vector3(cols+.2f, rows+.2f, 1);
            _gridMask.position=Vector3.zero;
            _gridBack.size=new Vector2(cols+.2f, rows+.2f);
            _gridBorder.size = _gridBack.size+Vector2.one/7.0f;
            _gridBack.transform.position=Vector3.zero;
            _gridBorder.transform.position=Vector2.zero;

            SetCameraSizeBaseOnTheBorderSize();
        }

       async void SetCameraSizeBaseOnTheBorderSize()
        {
            var camera = Camera.main;
            camera.orthographicSize = 5.0f;
            await Task.Yield();
            var topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var borderRight = _gridBorder.bounds.max.x;
            var borderTop = _gridBorder.bounds.max.y;
            var deltaX = borderRight - topRight.x;
            var deltaY = borderTop - topRight.y;
            var currentCameraSize =camera.orthographicSize ;
            if(deltaX<0 && deltaY<0)
                return;
                
            if (deltaX > deltaY)
            {
                camera.orthographicSize = currentCameraSize*(borderRight * 1.2f) / topRight.x;
                return;
            }
            camera.orthographicSize *= (borderTop * 1.2f) / topRight.y;
        }

        #endregion
    }
}