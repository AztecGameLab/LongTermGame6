using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;
using UnityTemplateProjects;

namespace Game.Enemy
{ 
    /// <summary>
    /// Allows an enemy to investigate an area after losing sight of enemy.
    /// </summary>
    public class EnemyInvestigateState : EnemyState
    {
        
        [Header("Settings")]
        
        [SerializeField] 
        [Tooltip("How close do we have to be to a destination before it counts as being visited.")]
        private float stopDistance = 0.1f;

        [Header("Dependencies")]
        
        [SerializeField] 
        [Tooltip("The agent used for pathfinding towards our target.")]
        private NavMeshAgent agent;
        
        [SerializeField] 
        [Tooltip("The system used to rotate towards our target.")]
        private RotationSystem rotationSystem;
        
        [SerializeField] 
        [Tooltip("The system we use to determine whether or not an attack target is detected.")]
        private TargetDetector attackTargetDetector;

        [Space(20f)] 
        
        [SerializeField] 
        private BehaviorTree investigateTree;
        public override string StateName => "Investigate";


        private Vector3 _lastKnowLocation;
        
        private void Awake()
        {
            investigateTree = BuildInvestigateTree();
        }

        #region Finite State Machine
        public override void OnStateEnter(EnemyStateManager enemy)
        {
            base.OnStateEnter(enemy);
            
            agent.ResetPath();
            investigateTree.Reset();
            
            rotationSystem.Deactivate();
            agent.updateRotation = true;
        }

        
        public override void OnStateExit(EnemyStateManager enemy)
        {
            base.OnStateExit(enemy);
            agent.ResetPath();
        }

        
        public override void OnStateUpdate(EnemyStateManager enemy)
        {
            if (attackTargetDetector.HasTarget)
                enemy.ChangeState(enemy.AttackState);
            else investigateTree.Tick();
        }
        
        #endregion


        #region Behaviour Tree

        private BehaviorTree BuildInvestigateTree()
        {
            return new BehaviorTree(gameObject);
        }

        private bool HasLastKnownLocation()
        {
            return _lastKnowLocation != null;
        }
        // ToDo: Find a way to retain information on the last position of enemy
        private TaskStatus RememberLastLocation()
        {
            
            return TaskStatus.Success;
        }
        
        private TaskStatus MoveTowardsLocation()
        {
            return agent.MoveTowards(_lastKnowLocation, stopDistance);
        }
        // ToDo: Make it spin
        private TaskStatus SpinAround()
        {
            return agent.MoveTowards(_lastKnowLocation, stopDistance);
        }
        #endregion
        
    }
}
