using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;
using UnityTemplateProjects;

namespace Game.Enemy
{
    /// <summary>
    /// Allows an enemy to travel between a looping set of points, stopping when it sees an enemy.
    /// </summary>
    
    public class EnemyPatrolState : EnemyState
    {
        [SerializeField] 
        [Tooltip("The positions this enemy will path-find between, starting from the top, moving down and looping.")]
        private Transform[] patrolPoints;
        
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
        private BehaviorTree patrolTree;

        // Internal Data
        
        private Vector3 _currentPatrolPoint;
        private int _currentPatrolPointIndex;

        public override string StateName => "Patrol";

        // Methods
        
        private void OnEnable()
        {
            patrolTree = BuildPatrolTree();
            
            if (HasPatrolPoints())
                FindNextPatrolPoint();
        }

        #region Finite State Machine

            public override void OnStateEnter(EnemyStateManager enemy)
            {
                base.OnStateEnter(enemy);

                agent.ResetPath();
                patrolTree.Reset();
                
                // Switch to using the NavMeshAgent for calculating rotation because its smooth.
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

                else patrolTree.Tick();
            }

        #endregion
        
        #region Behaviour Tree

            private BehaviorTree BuildPatrolTree()
            {
                return new BehaviorTreeBuilder(gameObject)
                
                    // Always move towards the next patrol point.
                
                    .RepeatForever()
                        .Sequence()
                            .Condition("Has Points", HasPatrolPoints)
                            .Do("Move To Point", MoveToPatrolPoint)
                            .Do("Find Next Point", FindNextPatrolPoint)
                        .End().End()
                
                    .Build();
            }

            private bool HasPatrolPoints()
            {
                return patrolPoints.Length > 0;
            }

            private TaskStatus FindNextPatrolPoint()
            {
                _currentPatrolPointIndex = (_currentPatrolPointIndex + 1) % patrolPoints.Length;
                _currentPatrolPoint = patrolPoints[_currentPatrolPointIndex].position;
                
                return TaskStatus.Success;
            }
            
            private TaskStatus MoveToPatrolPoint()
            {
                return agent.MoveTowards(_currentPatrolPoint, stopDistance);
            }

        #endregion
    }
}