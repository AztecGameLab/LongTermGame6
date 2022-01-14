using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace Game.Enemy
{
    // todo: generalize
    
    public class TestingEnemyBehaviourTree : MonoBehaviour
    {
        [SerializeField] private BehaviorTree tree;

        [SerializeField] private float stopDistance = 1f;
        [SerializeField] private float sightAngle = 45f;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private MovementSystem movementSystem;
        [SerializeField] private RotationSystem rotationSystem;
        
        private Vector3 VectorToPlayer => playerTransform.position - viewTransform.position;
        
        private void Awake()
        {
            movementSystem.Initialize();
            rotationSystem.Initialize();
            
            tree = new BehaviorTreeBuilder(gameObject)
                .Sequence("Chase Player")
                    .Condition("Can See Player", CanSeePlayer)
                    .Do("Move Towards Player", MoveTowardsPlayer)
                .End()
                .Build();
        }

        private bool CanSeePlayer()
        {
            if (Vector3.Angle(viewTransform.forward, VectorToPlayer.normalized) > sightAngle)
                return false;

            Ray rayToPlayer = new Ray(transform.position, VectorToPlayer);
            return Physics.Raycast(rayToPlayer, out var hitInfo) && hitInfo.transform.CompareTag("Player");
        }

        private TaskStatus MoveTowardsPlayer()
        {
            if (CanSeePlayer() == false)
            {
                movementSystem.UpdateMovement(Vector3.zero);
                return TaskStatus.Failure;
            }
            
            rotationSystem.Forward = VectorToPlayer;

            if (VectorToPlayer.magnitude > stopDistance)
            {
                movementSystem.UpdateMovement(VectorToPlayer.normalized);
                return TaskStatus.Continue;
            }

            movementSystem.UpdateMovement(Vector3.zero);
            return TaskStatus.Success;
        }

        private void Update()
        {
            tree.Tick();
        }
    }
}