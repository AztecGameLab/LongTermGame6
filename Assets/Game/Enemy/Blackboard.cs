using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class Blackboard : MonoBehaviour
    {
        private Dictionary<GameObject, Dictionary<string, object>> _data;

        private void Awake()
        {
            _data = new Dictionary<GameObject, Dictionary<string, object>>();
        }

        public void SetData<T>(GameObject target, string data, T value)
        {
            if (!_data.ContainsKey(target))
                _data.Add(target, new Dictionary<string, object>());
            
            _data[target][data] = value;
        }

        public T GetData<T>(GameObject target, string data)
        {
            if (_data.ContainsKey(target) && _data[target].ContainsKey(data))
                return (T) _data[target][data];

            return default;
        }
    }
}