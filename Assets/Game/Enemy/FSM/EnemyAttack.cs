using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;
using UnityTemplateProjects;

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
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Trigger damageTrigger;
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

            agent.ResetPath();
            attackTree.Reset();

            // Switch to using RotationSystem because its snappy and has constraints.            
            rotationSystem.Setup();
            agent.updateRotation = false;
        }

        public override void OnStateExit(EnemyStateManager enemy)
        {
            base.OnStateExit(enemy);
            agent.ResetPath();
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
                
                    // Always try to maintain LOS with the target.
                
                    .RepeatForever()
                        .Do(LookAtTarget).End()
                
                    // Always try to move into attack range, then attack the target.
                
                    .RepeatForever()
                        .Sequence()
                            .Do(MoveTowardsTarget)
                            .Do(AttackTarget)
                        .End().End()
                
                .End()
                .Build();
        }

        private TaskStatus LookAtTarget()
        {
            return rotationSystem.LookAt(_attackTarget.position, smoothTime);
        }

        private TaskStatus MoveTowardsTarget()
        {
            return agent.MoveTowards(_attackTarget.position, attackDistance);
        }

        // todo: move into its own "combat system" behaviour
        
        private TaskStatus AttackTarget()
        {
            if (TimeSinceAttack <= attackFrequencySeconds)
                return TaskStatus.Continue;
            
            // agent.isStopped = true;
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