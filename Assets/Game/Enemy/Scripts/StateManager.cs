using JetBrains.Annotations;
using UnityEngine;
using Display = UnityTemplateProjects.UI.Display;

namespace Game.Enemy
{
    public class StateManager : MonoBehaviour
    {
        [Header("Settings")] 
        
        [SerializeField]
        [Tooltip("The starting state for this enemy.")]
        private State defaultState;

        [SerializeField] 
        [Tooltip("The display used to show debug information.")]
        private Display debugDisplay;

        [SerializeField] private bool runOnStart = true;

        public State DefaultState => defaultState;
        public State CurrentState { get; private set; }
        public bool HasCurrentState { get; private set; }
        public bool IsRunning { get; private set; }

        private void Start()
        {
            if (runOnStart)
                StartMachine();
        }
        
        public void StartMachine()
        {
            if (IsRunning)
                return;
            
            IsRunning = true;
            ChangeState(DefaultState);
        }

        public void StopMachine()
        {
            if (!IsRunning)
                return;
            
            IsRunning = false;
            ChangeState(null);
        }

        private void Update()
        {
            if (!IsRunning || !HasCurrentState)
                return;
        
            CurrentState.OnStateUpdate(this);
            
            if (debugDisplay != null)
                debugDisplay.UpdateText(CurrentState.StateName);
        }

        [PublicAPI]
        public void ChangeState(State state)
        {
            if (HasCurrentState)
                CurrentState.OnStateExit(this);
    
            CurrentState = state;
            HasCurrentState = state != null;
            
            if (HasCurrentState) 
                state.OnStateEnter(this);
        }
    }
}