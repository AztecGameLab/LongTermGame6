using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public class LookTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent onLookStart;
        [SerializeField] private UnityEvent onLookEnd;
        [SerializeField] private float lookProximity = 0.75f;
        [SerializeField] private float lookDuration = 2f;
        [SerializeField] private float lookDistance = 5f;
        
        [SerializeField] 
        [Tooltip("The layers that should block line-of-sight.")]
        private LayerMask collisionLayerMask;
        
        private Transform _trackedTransform;
        private float _elapsedLookTime;
        private bool _isLookedAt;
        private bool _isActive;
        private bool _wasActive;
        
        private void Start()
        {
            if (Camera.main != null)
                _trackedTransform = Camera.main.transform;
        }

        private void UpdateIsLookedAt()
        {
            var lhs = (transform.position - _trackedTransform.position).normalized;
            var rhs = _trackedTransform.forward;
            
            _isLookedAt = Vector3.Dot(lhs, rhs) > lookProximity;
            
            Vector3 origin = transform.position;
            
            var vectorToTarget = _trackedTransform.position - origin;
            var rayToTarget = new Ray(origin, vectorToTarget);

            _isLookedAt = Vector3.Dot(lhs, rhs) > lookProximity && 
                          vectorToTarget.magnitude <= lookDistance &&
                          !Physics.Raycast(rayToTarget, vectorToTarget.magnitude ,collisionLayerMask.value, QueryTriggerInteraction.Ignore);
        }
        
        private void Update()
        {
            UpdateIsLookedAt();

            _elapsedLookTime = _isLookedAt 
                ? Mathf.Min(_elapsedLookTime + Time.deltaTime, lookDuration) 
                : Mathf.Max(_elapsedLookTime - Time.deltaTime, 0);

            _isActive = _elapsedLookTime >= lookDuration;
            
            if (_isActive && !_wasActive)
                onLookStart.Invoke();
            
            else if (!_isActive && _wasActive)
                onLookEnd.Invoke();
            
            _wasActive = _isActive;
        }
    }
}