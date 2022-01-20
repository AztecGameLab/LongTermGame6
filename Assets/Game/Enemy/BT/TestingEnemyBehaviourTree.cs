using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace Game.Enemy
{
    // todo: generalize
    
    public class TestingEnemyBehaviourTree : MonoBehaviour
    {
        // BT data

        [SerializeField] private BehaviorTree tree;
        [SerializeField] private MonoBehaviourTree attackBehaviour;
        [SerializeField] private MonoBehaviourTree alertBehaviour;
        [SerializeField] private MonoBehaviourTree idleBehaviour;
        
        private void Awake()
        {
            var attackTree = attackBehaviour.GetTree(gameObject);
            var alertTree = alertBehaviour.GetTree(gameObject);
            var idleTree = idleBehaviour.GetTree(gameObject);
            
            tree = new BehaviorTreeBuilder(gameObject)
                .Sequence("Root")
                    .Splice(attackTree)
                    // .Splice(alertTree)
                    .Splice(idleTree)
                .Build();
        }

        private void Update()
        {
            tree.Tick();
            tree.Reset();
        }
    }
}