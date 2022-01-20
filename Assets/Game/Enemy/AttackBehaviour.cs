using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class AttackBehaviour : MonoBehaviourTree
    {
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

        private Transform _attackTarget;
        private float _lastAttackTime;

        private float TimeSinceAttack => Time.time - _lastAttackTime;

        public override BehaviorTree GetTree(GameObject owner)
        {
            var tree = new BehaviorTreeBuilder(owner)
                .Selector()
                    .Condition("Target Hidden", TargetHidden)
                    .Sequence()
                        .Do(LookAtTarget)
                        .Selector()
                            .Condition("In Range", InAttackRange)
                            .Do("Move", MoveToTarget)
                        .End()
                        .Selector()
                            .Condition("Cooldown", OnAttackCooldown)
                            .Do("Attack", Attack)
                        .End()
                    .End()
                .End()
                .Build();
                
            return tree;
        }

        private bool TargetHidden()
        {
            return !attackSense.TryGetTarget(out _attackTarget);
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
            return TaskStatus.Success;
        }

        private TaskStatus MoveToTarget()
        {
            pathfinder.isStopped = false;
            
            return pathfinder.SetDestination(_attackTarget.position) 
                ? TaskStatus.Continue 
                : TaskStatus.Failure;
        }

        private TaskStatus Attack()
        {
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
    }
}