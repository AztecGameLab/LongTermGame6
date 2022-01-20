using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class EnemyIdle : EnemyState
    {
        [SerializeField] private NavMeshAgent pathfinder;
        [SerializeField] private RotationSystem rotationSystem;
        [SerializeField] private float stopDistance = 0.1f;
        [SerializeField] private float smoothTime = 1f;
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private Sense attackSense;
        
        [SerializeField] private BehaviorTree patrolTree;
        private int _currentPatrolPointIndex;
        private Vector3 _velocity;

        public override string StateName => "Idle";

        private void Awake()
        {
            patrolTree = BuildPatrolTree();
        }

        #region Finite State Machine

        public override void OnStateEnter(EnemyStateManager enemy)
        {
            base.OnStateEnter(enemy);
            pathfinder.ResetPath();
            pathfinder.isStopped = false;
            pathfinder.updateRotation = false;
            pathfinder.SetDestination(patrolPoints[_currentPatrolPointIndex].position);
            patrolTree.Reset();
        }

        public override void OnStateExit(EnemyStateManager enemy)
        {
            base.OnStateExit(enemy);
            pathfinder.ResetPath();
            patrolTree.Reset();
        }

        public override void OnStateUpdate(EnemyStateManager enemy)
        {
            if (attackSense.TryGetTarget(out _))
                enemy.ChangeState(enemy.AttackState);

            else patrolTree.Tick();
        }

        #endregion
        
        #region Behaviour Tree

        private BehaviorTree BuildPatrolTree()
        {
            return new BehaviorTreeBuilder(gameObject)
                .Parallel()
                    .Do(Rotate)
                    .Selector()
                        .Condition(Traveling)
                        .Do(SelectNewDestination)        
                    .End()
                .End()
                .Build();
        }
        
        private TaskStatus SelectNewDestination()
        {
            _currentPatrolPointIndex = (_currentPatrolPointIndex + 1) % patrolPoints.Length;
            Vector3 destination = patrolPoints[_currentPatrolPointIndex].position;
            
            return pathfinder.SetDestination(destination) 
                ? TaskStatus.Success 
                : TaskStatus.Failure;
        }

        private TaskStatus Rotate()
        {
            rotationSystem.Forward = Vector3.SmoothDamp(rotationSystem.Forward, pathfinder.steeringTarget - viewTransform.position, ref _velocity, smoothTime);
            return TaskStatus.Success;
        }

        private bool Traveling()
        {
            return pathfinder.remainingDistance > stopDistance;
        }        

        #endregion
    }
}