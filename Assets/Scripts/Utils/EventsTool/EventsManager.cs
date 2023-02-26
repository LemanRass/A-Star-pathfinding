using System;
using System.Collections.Generic;

namespace Utils.EventsTool
{
    public static class EventsManager
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers;

        static EventsManager()
        {
            _subscribers = new Dictionary<Type, List<Delegate>>();
        }
        
        public static void Publish<T>(T args) where T : struct
        {
            var key = typeof(T);
            if (!_subscribers.ContainsKey(key))    
                return;
            
            var list = _subscribers[typeof(T)];
            for (int i = 0; i < list.Count; i++)
                ((Action<T>)list[i]).Invoke(args);
        }

        public static void Subscribe<T>(Action<T> callback) where T : struct
        {
            var key = typeof(T);
            
            if (!_subscribers.ContainsKey(key))
            {
                _subscribers.Add(key, new List<Delegate> { callback });
                return;
            }
            
            _subscribers[key].Add(callback);
        }

        public static void UnSubscribe<T>(Action<T> callback) where T : struct
        {
            var key = typeof(T);

            if (_subscribers.ContainsKey(key))
                _subscribers[key].Remove(callback);
        }
    }
}