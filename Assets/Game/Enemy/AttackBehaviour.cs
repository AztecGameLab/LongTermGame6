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
        
        [Header("Dependencies")]
        [SerializeField] private RotationSystem rotationSystem;
        [SerializeField] private NavMeshAgent pathfinder;
        [SerializeField] private Trigger damageTrigger;
        [SerializeField] private Transform viewTransform;

        // todo ai general 1: turn into a "sense" behaviour that only populates on LOS
        
        [Header("Senses")]
        [SerializeField] private Transform target;

        private float _lastAttackTime;

        private float TimeSinceAttack => Time.time - _lastAttackTime;

        private void Awake()
        {
            pathfinder.updateRotation = false;
        }

        public override BehaviorTree GetTree(GameObject owner)
        {
            var tree = new BehaviorTreeBuilder(owner)
                .Selector()
                    .Condition("Has No Attack Target", HasNoAttackTarget)
                    .Sequence("Attack Target")
                        .Do(LookAtTarget)
                        .Selector()
                            .Condition("In Attack Range", InAttackRange)
                            .Do("Move To Target", MoveToTarget)
                        .End()
                        .Selector()
                            .Condition("On Attack Cooldown", OnAttackCooldown)
                            .Do("Attack", Attack)
                        .End()
                    .End()
                .End()
                .Build();
                
            return tree;
        }

        private bool HasNoAttackTarget()
        {
            return target == null;
        }

        private bool OnAttackCooldown()
        {
            return TimeSinceAttack <= attackFrequencySeconds;
        }

        private bool InAttackRange()
        {
            Vector3 targetPosition = target.position;
            Vector3 sourcePosition = viewTransform.position;
            float distanceToTarget = Vector3.Distance(targetPosition, sourcePosition);
            
            return distanceToTarget < attackDistance;
        }

        private TaskStatus LookAtTarget()
        {
            // todo ai attack 1: add smoothing or integrate IK into the look at function
            
            Vector3 vectorToPlayer = target.position - viewTransform.position;
            rotationSystem.Forward = vectorToPlayer;
            
            return TaskStatus.Success;
        }

        private TaskStatus MoveToTarget()
        {
            pathfinder.isStopped = false;
            pathfinder.SetDestination(target.position);
            return TaskStatus.Continue;
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