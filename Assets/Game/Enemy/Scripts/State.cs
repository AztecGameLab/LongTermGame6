using UnityEngine;

namespace Game.Enemy
{
    public abstract class State : MonoBehaviour
    {
        /// <summary>
        /// A human-readable name for this state. Displayed when debugging.
        /// </summary>
        public abstract string StateName { get; }
        
        /// <summary>
        /// Called when we switch to this state.
        /// </summary>
        /// <param name="parent">The object managing this state.</param>
        public virtual void OnStateEnter(StateManager parent) { }
        
        /// <summary>
        /// Called when we switch away from this state.
        /// </summary>
        /// <param name="parent">The object managing this state.</param>
        public virtual void OnStateExit(StateManager parent) { }
        
        /// <summary>
        /// Called every frame that this state is active.
        /// <remarks>This callback is useful for checking state transition conditions.</remarks>
        /// </summary>
        /// <param name="parent">The object managing this state.</param>
        public virtual void OnStateUpdate(StateManager parent) { }
    }
}