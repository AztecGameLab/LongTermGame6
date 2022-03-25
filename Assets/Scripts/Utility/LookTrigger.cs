using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public class LookTrigger : MonoBehaviour
    {
        [Header("Settings")]
        
        [SerializeField]
        [Tooltip("How close the player must be looking for events to trigger.")]
        private float lookProximity = 0.75f;
        
        [SerializeField]
        [Tooltip("How long the player must be looking at this for the event to trigger.")]
        private float lookDuration = 2f;
        
        [SerializeField]
        [Tooltip("How close the player must be to this object for this event to trigger.")]
        private float lookDistance = 5f;
        
        [SerializeField] 
        [Tooltip("The layers that should block line-of-sight.")]
        private LayerMask collisionLayerMask = 1;
        
        [Space(20f)]
        
        [SerializeField]
        public UnityEvent onLookStart = new UnityEvent();
        
        [SerializeField] 
        public UnityEvent onLookEnd = new UnityEvent();
        
        private Transform _trackedTransform;
        private float _elapsedLookTime;
        private bool _isActive;
        private bool _wasActive;
        
        private void Start()
        {
            if (Camera.main != null)
                _trackedTransform = Camera.main.transform;
        }

        private void Update()
        {
            _elapsedLookTime = IsLookedAt() 
                ? Mathf.Min(_elapsedLookTime + Time.deltaTime, lookDuration) 
                : Mathf.Max(_elapsedLookTime - Time.deltaTime, 0);
            
            UpdateEvents();
        }
        
        private bool IsLookedAt()
        {
            Vector3 origin = transform.position;
            Vector3 target = _trackedTransform.position;
            Vector3 vectorToTarget = target - origin;

            bool InRange() {
                return vectorToTarget.sqrMagnitude <= lookDistance * lookDistance;
            }
            
            bool LookedAt() {
                Vector3 directionToOrigin = (origin - target).normalized;
                Vector3 targetLookDirection = _trackedTransform.forward;
                return Vector3.Dot(directionToOrigin, targetLookDirection) > lookProximity;
            }

            bool Unobstructed() {
                Ray rayToTarget = new Ray(origin, vectorToTarget);
                return !Physics.Raycast(rayToTarget, vectorToTarget.magnitude ,collisionLayerMask.value, QueryTriggerInteraction.Ignore);
            }

            return InRange() && LookedAt() && Unobstructed();
        }

        private void UpdateEvents()
        {
            _isActive = _elapsedLookTime >= lookDuration;
            
            if (_isActive && !_wasActive)
                onLookStart.Invoke();
            
            else if (!_isActive && _wasActive)
                onLookEnd.Invoke();
            
            _wasActive = _isActive;
        }
    }
}