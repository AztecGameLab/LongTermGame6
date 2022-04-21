using UnityEngine;

namespace Utility
{
    public class RigidbodyUtil : MonoBehaviour
    {
        [SerializeField] private Rigidbody target;

        public void SetInterpolation(int value)
        {
            target.interpolation = (RigidbodyInterpolation) value;
        }
    }
}