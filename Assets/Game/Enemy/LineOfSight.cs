using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace Game.Enemy
{
    public class LineOfSight : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float range;
        
        [Header("Dependencies")]
        [SerializeField] private Transform viewTransform;
        [SerializeField] private Rigidbody viewRigidbody;

        public float VisibilityRange { get; set; }

        private void Awake()
        {
            VisibilityRange = range;
        }

        public bool CanSee(Vector3 position)
        {
            Vector3 origin = viewTransform.position;
            
            var vectorToTarget = position - origin;
            var rayToTarget = new Ray(origin, vectorToTarget);

            return Physics.Raycast(rayToTarget, out var hitInfo, VisibilityRange) && hitInfo.rigidbody == viewRigidbody;
        }
        
        public bool IsExposedTo(Transform source, float maxRange)
        {
            Vector3 sourcePosition = source.position;
            
            var vectorToTarget = viewTransform.position - sourcePosition;
            var rayToTarget = new Ray(sourcePosition, vectorToTarget);

            return Physics.Raycast(rayToTarget, out var hitInfo, maxRange) && hitInfo.rigidbody == viewRigidbody;
        }
    }
    
    public class VisibilityCondition : ConditionBase
    {
        public Type[] Targets;
        public Transform ViewSource;
        public float ViewRange;
        public LineOfSight Target;

        protected override bool OnUpdate()
        {
            float range = Mathf.Min(ViewRange, Target.VisibilityRange);
            return Target.IsExposedTo(ViewSource, range);
        }
    }
}