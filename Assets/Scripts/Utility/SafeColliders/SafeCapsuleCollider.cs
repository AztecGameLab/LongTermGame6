using UnityEngine;

namespace Utility
{
    public class SafeCapsuleCollider : SafeColliderGeneric<CapsuleCollider>
    {
        [SerializeField] private float padding = 0.05f;
        
        public override void CopyTo(CapsuleCollider target, CapsuleCollider original)
        {
            target.center = original.center;
            target.height = original.height - padding;
            target.radius = original.radius - padding;
            target.direction = original.direction;
        }
    }
}