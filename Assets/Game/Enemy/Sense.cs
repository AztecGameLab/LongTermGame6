using UnityEngine;

namespace Game.Enemy
{
    public abstract class Sense : MonoBehaviour
    {
        [SerializeField] private Transform[] targets;

        public abstract bool IsDetected(Transform target);
        
        public bool TryGetTarget(out Transform result)
        {
            result = null;

            foreach (Transform target in targets)
            {
                if (IsDetected(target))
                    result = target;
            }

            return result != null;
        }
    }
}