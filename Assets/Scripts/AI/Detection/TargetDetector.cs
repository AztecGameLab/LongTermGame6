using UnityEngine;

namespace Game.Enemy
{
    /// <summary>
    /// A class to generalize the different methods an AI might use to detect another object in the world.
    /// </summary>
    
    public abstract class TargetDetector : MonoBehaviour
    {
        [Tooltip("The objects this detector should be tracking and checking.")]
        [SerializeField]
        private Transform[] targets;

        [Header("Settings")]
        [SerializeField] 
        [Tooltip("How often should we update our current target (increase for better performance).")]
        private float pollingRate;
        
        // Internal Data
        
        /// <summary>
        /// Do we currently detect any targets?
        /// </summary>
        public bool HasTarget { get; private set; }

        /// <summary>
        /// The position of an object we current detect.
        /// </summary>
        public Transform Target => _currentTarget;
        private Transform _currentTarget;

        private float TimeSinceUpdate => Time.time - _lastUpdateTime; 
        private float _lastUpdateTime;

        // Methods
        
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