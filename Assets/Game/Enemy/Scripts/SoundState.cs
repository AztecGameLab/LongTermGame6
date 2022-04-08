using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;
using UnityTemplateProjects;

namespace Game.Enemy
{
    /// <summary>
    /// Allows an enemy to travel to were sound are heard.
    /// </summary>
    
    public class SoundState : State
    {
        
        [Header("Settings")]
        
        [SerializeField] 
        [Tooltip("How close do we have to be to a sound position before it counts as being visited.")]
        private float stopDistance = 0.25f;

        [Header("Dependencies")] 
        
        [SerializeField]
        private State idleState;
        
        [SerializeField]
        private State attackState;
        
        [SerializeField] 
        [Tooltip("The agent used for pathfinding towards our target.")]
        private NavMeshAgent agent;
        
        [SerializeField] 
        [Tooltip("The system used to rotate towards our target.")]
        private RotationSystem rotationSystem;
        
        [SerializeField] 
        [Tooltip("The system we use to determine whether or not an attack target is detected.")]
        private TargetDetector attackTargetDetector;
        
        [SerializeField] 
        [Tooltip("The system we use to determine whether or not an sound target is detected.")]
        private SoundDetector soundTargetDetector;
        
        [Space(20f)]
        
        [SerializeField] 
        private BehaviorTree soundTree;

        // Internal Data
        public override string StateName => "Sound";

        // Methods
        
        private void Awake()
        {
            soundTree = BuildPatrolTree();
        }

        #region Finite State Machine

            public override void OnStateEnter(StateManager parent)
            {
                agent.ResetPath();
                soundTree.Reset();
                
                // Switch to using the NavMeshAgent for calculating rotation because its smooth.
                rotationSystem.Deactivate();
                agent.updateRotation = true;
            }
            
            public override void OnStateUpdate(StateManager parent)
            {
                if (attackTargetDetector.HasTarget == false && soundTargetDetector.HasSound == false) //root state
                {
                    parent.ChangeState(idleState);
                }
                else if (attackTargetDetector.HasTarget && soundTargetDetector.HasSound) //attack state
                {
                    parent.ChangeState(attackState);
                }
                
                else soundTree.Tick();
            }
            
            public override void OnStateExit(StateManager parent)
            {
                soundTargetDetector.HasSound = false;
                agent.ResetPath();
            }
  
        #endregion
        
        #region Behaviour Tree

            private BehaviorTree BuildPatrolTree()
            {
                return new BehaviorTreeBuilder(gameObject)
                    // Always try to move to sound while theres a sound emitted.
                    .RepeatForever()
                        .Sequence()
                            .Do("Move Towards Target Sound", MoveTowardsTarget)
                            .Do("Change Sound Status", ChangeSoundStatus)
                        .End().End()
                    
                    .End()
                    .Build();
            }

            private TaskStatus MoveTowardsTarget()
            {
                return agent.MoveTowards(soundTargetDetector.HeardPosition, stopDistance);
            }

            private TaskStatus ChangeSoundStatus()
            {
                soundTargetDetector.HasSound = false;
                return TaskStatus.Success;
            }

        #endregion
    }
}