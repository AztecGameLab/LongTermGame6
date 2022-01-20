using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

namespace Game.Enemy
{
    public class AlertBehaviour : MonoBehaviourTree
    {
        public override BehaviorTree GetTree()
        {
            var tree = new BehaviorTreeBuilder(owner)
                .Do("Alert Placeholder", () => TaskStatus.Success)
                .Build();
                
            return tree;
        }
    }
}