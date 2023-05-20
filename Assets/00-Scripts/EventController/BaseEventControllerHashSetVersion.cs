using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3.EventController
{
    public abstract partial class BaseEventController
    {
           #region HashSetVersion

        #region EventCalsses

        public class SimpleEvent
        {
            #region Fields
            private event Action _action;
            #endregion

            #region Methods

            public void Add(Action listener)
            {
                _action+=listener;
            }

            public void Remove(Action listener)
            {
                _action-=listener;
            }

            public void Trigger()
            {
                _action?.Invoke();
            }

            #endregion
        }

        public class SimpleEvent<T>
        {
            #region Fields

            private event Action<T> _action;

            #endregion

            #region Methods

            public void Add(Action<T> listener)
            {
                _action+=listener;
            }

            public void Remove(Action<T> listener)
            {
                _action-=listener;
            }

            public void Trigger(T value)
            {
                _action?.Invoke(value);
            }
            #endregion
        }

        public class HashFuncEvent<T>
        {
            #region Fields

            private readonly HashSet<Func<T>> _listeners = new();

            #endregion

            #region Methods

            public void Add(Func<T> listener)
            {
                if (_listeners.Contains(listener))
                    return;
                _listeners.Add(listener);
            }

            public void Remove(Func<T> listener)
            {
                if (!_listeners.Contains(listener))
                    return;
                _listeners.Remove(listener);
            }

            public List<T> Trigger()
            {
                if (_listeners.Count == 0)
                    return default;
                var outPut = new List<T>(_listeners.Count);
                /*for (int i = 0,e=_listeners.Count; i < e; i++)*/
                foreach (var listener in _listeners.ToList())
                {
                    if (listener == default)
                        continue;
                    outPut.Add(listener.Invoke());
                }

                return outPut;
            }

            #endregion
        }

        public class HashFuncEvent<TIn, TOut>
        {
            #region Fields

            private readonly HashSet<Func<TIn, TOut>> _listeners = new();

            #endregion

            #region Methods

            public void Add(Func<TIn, TOut> listener)
            {
                if (_listeners.Contains(listener))
                    return;
                _listeners.Add(listener);
            }

            public void Remove(Func<TIn, TOut> listener)
            {
                if (!_listeners.Contains(listener))
                    return;
                _listeners.Remove(listener);
            }

            public List<TOut> Trigger(TIn input)
            {
                if (_listeners.Count == 0)
                    return default;
                var outPut = new List<TOut>(_listeners.Count);
                foreach (var listener in _listeners.ToList())
                {
                    if (listener == default)
                        continue;
                    outPut.Add(listener.Invoke(input));
                }

                return outPut;
            }

            #endregion
        }

        #endregion

        #endregion
    }
}