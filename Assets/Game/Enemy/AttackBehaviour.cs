using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class AttackBehaviour : MonoBehaviourTree
    {
        [SerializeField] private NavMeshAgent pathfinder;
        [SerializeField] private Transform target;
        [SerializeField] private Transform source;
        [SerializeField] private Trigger damageTrigger;

        private float _lastAttackTime;

        private float TimeSinceAttack => Time.time - _lastAttackTime;
        
        public override BehaviorTree GetTree()
        {
            var tree = new BehaviorTreeBuilder(owner)
                .Selector()
                    .Condition("Has No Attack Target", HasNoAttackTarget)
                    .Sequence("Attack Target")
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
            return TimeSinceAttack <= 1f;
        }

        private bool InAttackRange()
        {
            Vector3 targetPosition = target.position;
            Vector3 sourcePosition = source.position;
            float distanceToTarget = Vector3.Distance(targetPosition, sourcePosition);
            
            return distanceToTarget < 2;
        }

        private TaskStatus MoveToTarget()
        {
            if (!pathfinder.isStopped && pathfinder.remainingDistance < 2)
            {
                pathfinder.isStopped = true;
                return TaskStatus.Success;
            }
            
            pathfinder.isStopped = false;
            pathfinder.SetDestination(target.position);
            return TaskStatus.Continue;
        }

        private TaskStatus Attack()
        {
            _lastAttackTime = Time.time;

            foreach (Rigidbody occupant in damageTrigger.Occupants)
            {
                if (occupant != null && occupant.TryGetComponent(out Health health))
                    health.Damage(0.25f);
            }
            
            return TaskStatus.Success;
        }
    }
}