using UnityEngine;

namespace Game.Enemy
{
    public class Vision : Sense
    {
        [Header("Settings")] 
        [SerializeField] private float viewAngle = 45f;
        [SerializeField] private float viewRange = 10f;
        [SerializeField] private LayerMask collisionLayerMask;
        
        [Header("Dependencies")]
        [SerializeField] private Transform viewTransform;

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