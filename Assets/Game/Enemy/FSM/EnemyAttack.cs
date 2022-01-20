using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class EnemyAttack : EnemyState 
    {
        public override string StateName => "Attack";
        
        [Header("Settings")] 
        [SerializeField, Range(0, 1)] private float attackDamage = 0.25f;
        [SerializeField] private float attackFrequencySeconds = 1f;
        [SerializeField] private float attackDistance = 1.5f;
        [SerializeField] private float smoothTime = 1f;
        
        [Header("Dependencies")]
        [SerializeField] private RotationSystem rotationSystem;
        [SerializeField] private NavMeshAgent pathfinder;
        [SerializeField] private Trigger damageTrigger;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private Sense attackSense;

        [SerializeField] private BehaviorTree attackTree;
        
        private Transform _attackTarget;
        private float _lastAttackTime;

        private float TimeSinceAttack => Time.time - _lastAttackTime;

        private void Awake()
        {
            attackTree = BuildAttackTree();
        }

        #region Finite State Machine

        public override void OnStateEnter(EnemyStateManager enemy)
        {
            base.OnStateEnter(enemy);
            pathfinder.ResetPath();
            pathfinder.isStopped = false;
            pathfinder.updateRotation = false;
            attackTree.Reset();
        }

        public override void OnStateExit(EnemyStateManager enemy)
        {
            base.OnStateExit(enemy);
            pathfinder.ResetPath();
            attackTree.Reset();
        }
        
        public override void OnStateUpdate(EnemyStateManager enemy)
        {
            if (!attackSense.TryGetTarget(out _attackTarget))
                enemy.ChangeState(enemy.IdleState);

            else attackTree.Tick();
        }

        #endregion

        #region Behaviour Tree

        public BehaviorTree BuildAttackTree()
        {
            return new BehaviorTreeBuilder(gameObject)
                .Parallel()
                    .Do(LookAtTarget)
                    .RepeatForever()
                        .Sequence()
                            .Do(MoveToTarget)
                            .Do(Attack)
                        .End()
                    .End()
                .End()
                .Build();
        }

        private bool OnAttackCooldown()
        {
            return TimeSinceAttack <= attackFrequencySeconds;
        }

        private bool InAttackRange()
        {
            Vector3 targetPosition = _attackTarget.position;
            Vector3 sourcePosition = viewTransform.position;
            float distanceToTarget = Vector3.Distance(targetPosition, sourcePosition);
            
            return distanceToTarget < attackDistance;
        }

        private Vector3 _velocity;

        private TaskStatus LookAtTarget()
        {
            rotationSystem.Forward = Vector3.SmoothDamp(rotationSystem.Forward, _attackTarget.position - viewTransform.position, ref _velocity, smoothTime);
            return TaskStatus.Continue;
        }

        private TaskStatus MoveToTarget()
        {
            if (InAttackRange())
                return TaskStatus.Success;
            
            pathfinder.isStopped = false;

            return pathfinder.SetDestination(_attackTarget.position)
                ? TaskStatus.Continue
                : TaskStatus.Failure;
        }

        private TaskStatus Attack()
        {
            if (OnAttackCooldown())
                return TaskStatus.Failure;
            
            pathfinder.isStopped = true;
            _lastAttackTime = Time.time;

            // todo ai attack 2: add stronger damage feel (knock-back, sound effects, ect.)
            // todo ai attack 3: pass damage amount to FMOD for changing the impact of the sound
            
            foreach (Rigidbody occupant in damageTrigger.Occupants)
            {
                if (occupant.TryGetComponent(out Health health))
                    health.Damage(attackDamage);
            }

            return TaskStatus.Success;
        }

        #endregion
    }
}