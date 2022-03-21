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
        
        public State DefaultState => defaultState;
        public State CurrentState { get; private set; }
        
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

        [PublicAPI]
        public void ChangeState(State state)
        {
            CurrentState.OnStateExit(this);
            CurrentState = state;
            
            if (state != null) 
                state.OnStateEnter(this);
        }
    }
}