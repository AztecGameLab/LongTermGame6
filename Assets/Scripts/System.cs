using ConsoleUtility;
using JetBrains.Annotations;
using UnityEngine;

namespace MyNamespace
{
    public abstract class System : MonoBehaviour
    {
        [PublicAPI]
        public virtual void Initialize()
        {
            Console.Log("System", $"Initialized {GetType().Name}");
        }
    }
}