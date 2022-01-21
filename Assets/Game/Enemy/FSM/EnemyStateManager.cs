using UnityEngine;

namespace Game.Enemy
{
    public class EnemyStateManager : MonoBehaviour
    {
        [Header("Settings")]
        
        [SerializeField]
        [Tooltip("The starting state for this enemy.")]
        private EnemyState defaultState;

        [SerializeField] 
        [Tooltip("Log debug information about this enemy to the console.")]
        private bool showDebug;
        
        [Header("Dependencies")]
        
        [SerializeField]
        [Tooltip("Behaviour for when no targets can be found.")]
        private EnemyState idleState;
        
        [SerializeField] 
        [Tooltip("Behaviour for when a target is found.")]
        private EnemyState attackState;

        // Internal Data

        public bool ShowDebug => showDebug;

        public EnemyState DefaultState => defaultState;
        public EnemyState IdleState => idleState;
        public EnemyState AttackState => attackState;
        
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
        }

        public void ChangeState(EnemyState state)
        {
            CurrentState.OnStateExit(this);
            CurrentState = state;
            CurrentState.OnStateEnter(this);
        }
    }
}