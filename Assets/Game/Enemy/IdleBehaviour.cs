using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class IdleBehaviour : MonoBehaviourTree
    {
        [SerializeField] private NavMeshAgent pathfinder;
        [SerializeField] private RotationSystem rotationSystem;
        [SerializeField] private float stopDistance = 0.1f;
        [SerializeField] private float smoothTime = 1f;
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private Sense attackSense;
        
        private int _currentPatrolPointIndex;

        private void Awake()
        {
            pathfinder.updateRotation = false;
        }

        public override BehaviorTree GetTree(GameObject owner)
        {
            var tree = new BehaviorTreeBuilder(owner)
                .Selector()
                    .Condition(() => attackSense.TryGetTarget(out _))
                    .Sequence()
                        .Do(Rotate)
                        .Selector()
                            .Condition(Traveling)
                            .Do(SelectNewDestination)        
                        .End()
                    .End()
                .End()
                .Build();
                
            return tree;
        }

        private TaskStatus SelectNewDestination()
        {
            _currentPatrolPointIndex = (_currentPatrolPointIndex + 1) % patrolPoints.Length;
            Vector3 destination = patrolPoints[_currentPatrolPointIndex].position;
            
            return pathfinder.SetDestination(destination) 
                ? TaskStatus.Continue 
                : TaskStatus.Failure;
        }

        private Vector3 _velocity;

        private TaskStatus Rotate()
        {
            rotationSystem.Forward = Vector3.SmoothDamp(rotationSystem.Forward, pathfinder.steeringTarget - viewTransform.position, ref _velocity, smoothTime);
            return TaskStatus.Success;
        }

        private bool Traveling()
        {
            return pathfinder.remainingDistance > stopDistance;
        }
    }
}