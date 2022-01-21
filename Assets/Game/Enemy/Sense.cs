using System;
using System.Linq;
using UnityEngine;

namespace Game.Enemy
{
    public abstract class Sense : MonoBehaviour
    {
        [SerializeField] private Transform[] targets;
        [SerializeField] private float pollingRate;

        public bool HasTarget { get; private set; }

        public Transform Target => _currentTarget;
        private Transform _currentTarget;

        private float TimeSinceUpdate => Time.time - _lastUpdateTime; 
        private float _lastUpdateTime;

        
        private void Update()
        {
            if (TimeSinceUpdate > pollingRate)
            {
                _lastUpdateTime = Time.time;
                HasTarget = TryGetTarget(out _currentTarget);
            }
        }
        
        private bool TryGetTarget(out Transform result)
        {
            result = null;

            foreach (Transform target in targets)
            {
                if (IsDetected(target))
                    result = target;
            }

            return result != null;
        }
        
        protected abstract bool IsDetected(Transform target);
    }
}