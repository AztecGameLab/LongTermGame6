using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace Game.Enemy
{
    public abstract class MonoBehaviourTree : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] protected GameObject owner;
        
        public abstract BehaviorTree GetTree();
    }
}