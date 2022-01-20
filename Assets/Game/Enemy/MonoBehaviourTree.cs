using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace Game.Enemy
{
    public abstract class MonoBehaviourTree : MonoBehaviour
    {
        public abstract BehaviorTree GetTree(GameObject owner);
    }
}