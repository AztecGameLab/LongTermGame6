using UnityEngine;

namespace Utility
{
    public class SafeSphereCollider : SafeColliderGeneric<SphereCollider>
    {
        [SerializeField] private float padding = 0.05f;
        
        public override void CopyTo(SphereCollider target, SphereCollider original)
        {
            target.center = original.center;
            target.radius = original.radius - padding;
        }
    }
}