using UnityEngine;

namespace Utility
{
    public abstract class SafeCollider : MonoBehaviour
    {
        private Collider _collider;
        private Trigger _trigger;

        public bool IsClear => _trigger.Colliders.Count <= 0;
        
        private void Start()
        {
            var triggerObject = new GameObject("Safe Trigger");
            
            triggerObject.transform.SetParent(transform);
            triggerObject.transform.localPosition = Vector3.zero;
            triggerObject.transform.localRotation = Quaternion.identity;

            _collider = GetCollider();
            _trigger = AddTrigger(triggerObject);
        }

        public void SafeEnable()
        {
            if (IsClear)
                _collider.enabled = true;
        }

        public void Disable()
        {
            _collider.enabled = false;
        }

        public abstract Collider GetCollider();
        public abstract Trigger AddTrigger(GameObject target);
    }
}