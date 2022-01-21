using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;
using UnityTemplateProjects;

namespace Game.Enemy
{
    public class EnemyIdle : EnemyState
    {
        public override string StateName => "Idle";

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private RotationSystem rotationSystem;
        [SerializeField] private float stopDistance = 0.1f;
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private Sense attackSense;
        
        [SerializeField] private BehaviorTree patrolTree;
        
        private Vector3 _currentPatrolPoint;
        private int _currentPatrolPointIndex;
        
        private void Awake()
        {
            patrolTree = BuildPatrolTree();
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
            if (attackSense.HasTarget)
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
                        .Do("Move To Point", MoveToPatrolPoint)
                        .Do("Find Next Point", FindNextPatrolPoint)
                    .End().End()
            
                .Build();
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