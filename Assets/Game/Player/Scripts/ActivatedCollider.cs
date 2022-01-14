using NaughtyAttributes;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Collider))]
    public class ActivatedCollider : MonoBehaviour
    {
        [SerializeField, Layer] private int excludeLayer;
        [SerializeField] private Behaviour targetCollider;
        [SerializeField] private bool showDebug;

        private bool _isObstructed;
        private bool _objectInTrigger;
        
        [Button("Activate")]
        public bool TryActivate()
        {
            if (_isObstructed)
                return false;

            targetCollider.enabled = true;
            return true;
        }
        
        [Button]
        public void Deactivate()
        {
            targetCollider.enabled = false;
        }

        private void FixedUpdate()
        {
            _isObstructed = _objectInTrigger;
            _objectInTrigger = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != excludeLayer)
                _isObstructed = true;
        }

        private void OnGUI()
        {
            if (showDebug)
                GUILayout.Label($"Is obstructed: {_isObstructed}.");
        }
    }
}