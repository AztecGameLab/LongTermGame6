using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace Game.Enemy
{
    public class IdleBehaviour : MonoBehaviourTree
    {
        public override BehaviorTree GetTree(GameObject owner)
        {
            var tree = new BehaviorTreeBuilder(owner)
                .Do("Idle Placeholder", () => TaskStatus.Success)
                .Build();
                
            return tree;
        }
    }
}