using UnityEngine;

namespace Utility
{
    public abstract class SafeColliderGeneric<T> : SafeCollider where T : Collider
    {
        public override Collider GetCollider()
        {
            return GetComponent<T>();
        }

        public override Trigger AddTrigger(GameObject target)
        {
            var originalCollider = GetComponent<T>();
            var targetCollider = target.AddComponent<T>();
            targetCollider.isTrigger = true;
            
            CopyTo(targetCollider, originalCollider);

            return target.AddComponent<Trigger>();
        }
        
        public abstract void CopyTo(T target, T original);
    }
}