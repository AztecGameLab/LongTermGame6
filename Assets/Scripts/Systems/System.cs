using JetBrains.Annotations;
using UnityEngine;

namespace MyNamespace
{
    public abstract class System : MonoBehaviour
    {
        [PublicAPI]
        public virtual void Initialize()
        {
            // Allow children to implement their own startup logic.
        }
    }
}