using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace Game.Enemy
{
    public static class BehaviourTreeBuilderExtensions
    {
        public static BehaviorTreeBuilder IsVisible(this BehaviorTreeBuilder builder, 
            Transform viewSource, 
            LineOfSight target, 
            float viewRange, 
            string name = "Is Visible")
        {
            var node = new VisibilityCondition
            {
                Name = name, 
                Target = target, 
                ViewRange = viewRange, 
                ViewSource = viewSource
            };
            
            return builder.AddNode(node);
        }
    }
}