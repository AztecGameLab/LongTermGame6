using UnityEngine;

namespace Game.Enemy
{
    /// <summary>
    /// Detects objects if they are not blocked by other objects on a certain layer.
    /// </summary>
    
    public class VisionDetector : TargetDetector
    {
        [SerializeField] 
        [Range(0, 360)]
        [Tooltip("How wide should our view be (like FOV).")]
        private float viewAngle = 45f;
        
        [SerializeField] 
        [Tooltip("Targets that are further than this distance will not be detected.")]
        private float viewRange = 10f;
        
        [SerializeField] 
        [Tooltip("The layers that should block line-of-sight.")]
        private LayerMask collisionLayerMask;
        
        [Header("Dependencies")]
        [SerializeField]
        [Tooltip("Where we should start when calculating line-of-sight.")]
        private Transform viewTransform;

        // Methods
        
        protected override bool IsDetected(Transform target)
        {
            Vector3 origin = viewTransform.position;
            
            var vectorToTarget = target.position - origin;
            var rayToTarget = new Ray(origin, vectorToTarget);

            if (vectorToTarget.magnitude > viewRange || Vector3.Angle(vectorToTarget, viewTransform.forward) > viewAngle)
                return false;
            
            return !Physics.Raycast(rayToTarget, vectorToTarget.magnitude ,collisionLayerMask.value, QueryTriggerInteraction.Ignore);
        }
    }
}