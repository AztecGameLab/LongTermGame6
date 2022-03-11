using UnityEngine;
using Display = UnityTemplateProjects.UI.Display;

namespace Game.Enemy
{
    public class EnemyStateManager : MonoBehaviour
    {
        [Header("Settings")]
        
        [SerializeField]
        [Tooltip("The starting state for this enemy.")]
        private EnemyState defaultState;

        [SerializeField] 
        [Tooltip("The display used to show debug information.")]
        private Display debugDisplay;
        
        [Header("Dependencies")]
        
        [SerializeField]
        [Tooltip("Behaviour for when no targets can be found.")]
        private EnemyState idleState;
        
        [SerializeField] 
        [Tooltip("Behaviour for when a target is found.")]
        private EnemyState attackState;
        
        [SerializeField] 
        [Tooltip("Behaviour for when a target is out of sight.")]
        private EnemyState investigateState;

        // Internal Data

        public EnemyState DefaultState => defaultState;
        public EnemyState IdleState => idleState;
        public EnemyState AttackState => attackState;

        public EnemyState InvestigateState => investigateState;
        
        public EnemyState CurrentState { get; private set; }
        
        // Methods
        
        private void Start()
        {
            CurrentState = DefaultState;
            CurrentState.OnStateEnter(this);
        }

        private void Update()
        {
            CurrentState.OnStateUpdate(this);
            
            if (debugDisplay != null)
                debugDisplay.UpdateText(CurrentState.StateName);
        }

        public void ChangeState(EnemyState state)
        {
            CurrentState.OnStateExit(this);
            CurrentState = state;
            CurrentState.OnStateEnter(this);
        }
    }
}