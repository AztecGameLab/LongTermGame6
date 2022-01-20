using UnityEngine;

namespace Game.Enemy
{
    public class EnemyStateManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool showDebug;
        
        [SerializeField] private EnemyIdle idleState;
        [SerializeField] private EnemyAlert alertState;
        [SerializeField] private EnemyAttack attackState;

        private EnemyState _currentState;

        public EnemyIdle IdleState => idleState;
        public EnemyAlert AlertState => alertState;
        public EnemyAttack AttackState => attackState;
        public bool ShowDebug => showDebug;

        private void Start()
        {
            ChangeState(idleState);
        }

        #if UNITY_EDITOR
        
        private void OnDrawGizmos()
        {
            string currentStateName = _currentState != null ? _currentState.StateName : "Uninitialized";
            UnityEditor.Handles.Label(transform.position, $"State: {currentStateName}");
        }
        
        #endif

        private void Update()
        {
            _currentState.OnStateUpdate(this);
        }

        public void ChangeState(EnemyState state)
        {
            if (_currentState != null)
                _currentState.OnStateExit(this);
            
            _currentState = state;
            
            if (_currentState != null)
                _currentState.OnStateEnter(this);
        }
    }
}