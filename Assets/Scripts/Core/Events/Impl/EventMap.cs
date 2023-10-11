using System;
using System.Collections.Generic;
using System.Linq;
using Core.Commands.Binding.Impl;

namespace Core.Events.Impl
{
    public sealed class EventMap : IEventMap
    {
        private readonly CommandBinder _commandBinder;
        private readonly Dictionary<EventBase, object> _listeners;

        public EventMap(CommandBinder commandBinder)
        {
            _listeners = new Dictionary<EventBase, object>();
            _commandBinder = commandBinder;
        }
        
        public void Map(Event @event, Action action)
        {
            if (!_listeners.TryGetValue(@event, out var listener) || listener == null)
            {
                _listeners[@event] = action;
                return;
            }

            var listenerAction = (Action)listener;
            if (!listenerAction.GetInvocationList().Contains(action))
                _listeners[@event] = listenerAction + action;
        }

        public void Map<T1>(Event<T1> @event, Action<T1> action)
        {
            if (!_listeners.TryGetValue(@event, out var listener) || listener == null)
            {
                _listeners[@event] = action;
                return;
            }

            var listenerAction = (Action<T1>)listener;
            if (!listenerAction.GetInvocationList().Contains(action))
                _listeners[@event] = listenerAction + action;
        }

        public void Map<T1, T2>(Event<T1, T2> @event, Action<T1, T2> action)
        {
            if (!_listeners.TryGetValue(@event, out var listener) || listener == null)
            {
                _listeners[@event] = action;
                return;
            }

            var listenerAction = (Action<T1, T2>)listener;
            if (!listenerAction.GetInvocationList().Contains(action))
                _listeners[@event] = listenerAction + action;
        }

        public void UnMap(Event @event, Action action)
        {
            if (!_listeners.TryGetValue(@event, out var listener) || listener == null)
                return;

            var listenerAction = (Action)listener;
            if (listenerAction.GetInvocationList().Contains(action))
                _listeners[@event] = listenerAction - action;
        }

        public void UnMap<T1>(Event<T1> @event, Action<T1> action)
        {
            if (!_listeners.TryGetValue(@event, out var listener) || listener == null)
                return;

            var listenerAction = (Action<T1>)listener;
            if (listenerAction.GetInvocationList().Contains(action))
                _listeners[@event] = listenerAction - action;
        }

        public void UnMap<T1, T2>(Event<T1, T2> @event, Action<T1, T2> action)
        {
            if (!_listeners.TryGetValue(@event, out var listener) || listener == null)
                return;

            var listenerAction = (Action<T1, T2>)listener;
            if (listenerAction.GetInvocationList().Contains(action))
                _listeners[@event] = listenerAction - action;
        }

        public void Dispatch(Event @event)
        {
            if (_listeners.TryGetValue(@event, out var listener))
            {
                var listenerAction = (Action)listener;
                listenerAction?.Invoke();
            }
            
            _commandBinder.ProcessEvent(@event);
        }

        public void Dispatch<T1>(Event<T1> @event, T1 param01)
        {
            if (!_listeners.TryGetValue(@event, out var listener)) 
                return;
            
            var listenerAction = (Action<T1>)listener;
            listenerAction?.Invoke(param01);
        }

        public void Dispatch<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02)
        {
            if (!_listeners.TryGetValue(@event, out var listener)) 
                return;
            
            var listenerAction = (Action<T1, T2>)listener;
            listenerAction?.Invoke(param01, param02);
        }
    }
}
