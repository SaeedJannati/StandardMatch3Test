using System;
using Match3.Auxiliary;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.General
{
    public class Swipeable : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
    {
        #region Fields

        public Action<Direction> onSwipe ;
        private Camera _camera;
        private Vector3 _initDragPos=new();
        #endregion

        #region Unity actions

        private void Awake()
        {
            _camera=Camera.main;
        }

        #endregion
        #region Methods

        void CheckForSwipe(Vector3 deltaDragPos)
        {
            var deltaX = deltaDragPos.x;
            var deltaY = deltaDragPos.y;

            if (deltaX * deltaX < deltaY * deltaY)
            {
                if (deltaY > 0)
                {
                    onSwipe?.Invoke(Direction.Up);
                    return;
                }
                onSwipe?.Invoke(Direction.Down);
                return;
            }

            if (deltaX > 0)
            {
                onSwipe?.Invoke(Direction.Right);
                return;
            }
            onSwipe?.Invoke(Direction.Left);
        }

        Vector3 GetWorldPoint(PointerEventData eventData)
        {
            Vector3 pos = eventData.position;
            pos.z = 10;
            return _camera.ScreenToWorldPoint(pos);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            var deltaPos=GetWorldPoint(eventData)-_initDragPos;
            CheckForSwipe(deltaPos);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _initDragPos = GetWorldPoint(eventData);
        }
        #endregion

  
    }
    [Serializable]
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
