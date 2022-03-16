using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;
using UnityTemplateProjects;

namespace Game.Enemy
{
    /// <summary>
    /// Allows an enemy to chase and damage a target, stopping when it loses its target.
    /// </summary>
    
    public class AttackState : State 
    {
        [Header("Settings")]
        
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("What percent of the player's health should we take away when hitting them.")]
        private float attackDamage = 0.25f;
        
        [SerializeField] 
        [Tooltip("How often we can attack the player, in seconds.")]
        private float attackFrequency = 1f;
        
        [SerializeField]
        [Tooltip("How close we should get to the player before attacking.")]
        private float attackDistance = 1.5f;
        
        [SerializeField] 
        [Tooltip("How quickly should we turn to face the player, larger values take longer.")]
        private float smoothTime = 1f;

        [Header("Dependencies")] 
        
        [SerializeField]
        private State idleState;
        
        [SerializeField] 
        [Tooltip("The system used to rotate towards our target.")]
        private RotationSystem rotationSystem;
        
        [SerializeField] 
        [Tooltip("The agent used for pathfinding towards our target.")]
        private NavMeshAgent agent;
        
        [SerializeField] 
        [Tooltip("The trigger in which objects will be hurt from our attacks.")]
        private Trigger damageTrigger;
        
        [SerializeField]
        [Tooltip("The system we use to determine whether or not an attack target is detected.")]
        private TargetDetector attackTargetDetector;

        [Space(20f)]
        
        [SerializeField]
        [Tooltip("The behaviour tree that represents this entity.")]
        private BehaviorTree attackTree;

        // Internal Data
        
        public override string StateName => "Attack";

        private float TimeSinceAttack => Time.time - _lastAttackTime;
        private float _lastAttackTime;

        private Vector3 TargetPosition => attackTargetDetector.HasTarget 
            ? attackTargetDetector.Target.position 
            : Vector3.zero;
        
        // Methods
        
        private void Awake()
        {
            attackTree = BuildAttackTree();
        }

        #region Finite State Machine

            public override void OnStateEnter(StateManager parent)
            {
                agent.ResetPath();
                attackTree.Reset();

                // Switch to using RotationSystem because its snappy and has constraints.            
                rotationSystem.Activate();
                agent.updateRotation = false;
            }

            public override void OnStateUpdate(StateManager parent)
            {
                if (attackTargetDetector.HasTarget == false)
                    parent.ChangeState(idleState);

                else attackTree.Tick();
            }
            
            public override void OnStateExit(StateManager parent)
            {
                agent.ResetPath();
            }

        #endregion

        #region Behaviour Tree

            public BehaviorTree BuildAttackTree()
            {
                return new BehaviorTreeBuilder(gameObject)
                    .Parallel()
                    
                    // Always try to maintain LOS with the target.
                    
                    .RepeatForever()
                        .Do("Look at Target", LookAtTarget).End()
                    
                    // Always try to move into attack range, then attack the target.
                    
                    .RepeatForever()
                        .Sequence()
                            .Do("Move Towards Target", MoveTowardsTarget)
                            .Do("Attack Target", AttackTarget)
                        .End().End()
                    
                    .End()
                    .Build();
            }

            private TaskStatus LookAtTarget()
            {
                return rotationSystem.LookAt(TargetPosition, smoothTime);
            }

            private TaskStatus MoveTowardsTarget()
            {
                return agent.MoveTowards(TargetPosition, attackDistance);
            }

            // todo: move into its own "combat system" behaviour
            
            private TaskStatus AttackTarget()
            {
                if (TimeSinceAttack <= attackFrequency)
                    return TaskStatus.Continue;
                
                _lastAttackTime = Time.time;

                // todo ai attack 2: add stronger damage feel (knock-back, sound effects, ect.)
                // todo ai attack 3: pass damage amount to FMOD for changing the impact of the sound
                
                foreach (Rigidbody occupant in damageTrigger.Rigidbodies)
                {
                    if (occupant.TryGetComponent(out Health health))
                        health.Damage(attackDamage);
                }

                return TaskStatus.Success;
            }

        #endregion
    }
}