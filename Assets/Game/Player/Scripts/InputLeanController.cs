using System;
using UnityEngine;

namespace Player.Lean
{
    /// <summary>
    /// Controls the leaning system with input.
    /// </summary>
    
    public class InputLeanController : InputController<LeanSystem>
    {
        public enum LeanMode { Hold, Toggle }

        [SerializeField] 
        [Tooltip("Hold will only lean while the button is held. Toggle will remain leaning until pressed again.")]
        private LeanMode mode;
        
        private void Update()
        {
            switch (mode)
            {
                case LeanMode.Hold:
                    HandleHold();
                    break;
                case LeanMode.Toggle:
                    HandleToggle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleToggle()
        {
            if (Input.GetKeyDown(controls.leanRight))
            {
                system.TargetState = system.CurrentState == LeanSystem.LeanState.Right 
                    ? LeanSystem.LeanState.Center 
                    : LeanSystem.LeanState.Right;
            }

            if (Input.GetKeyDown(controls.leanLeft))
            {
                system.TargetState = system.CurrentState == LeanSystem.LeanState.Left 
                    ? LeanSystem.LeanState.Center 
                    : LeanSystem.LeanState.Left;
            }
        }

        private void HandleHold()
        {
            int holdStateCounter = 0;

            if (Input.GetKey(controls.leanRight))
                holdStateCounter++;
            
            if (Input.GetKey(controls.leanLeft))
                holdStateCounter--;

            system.TargetState = holdStateCounter switch
            {
                -1 => LeanSystem.LeanState.Left,
                0 => LeanSystem.LeanState.Center,
                1 => LeanSystem.LeanState.Right,
                
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void OnDisable()
        {
            system.TargetState = LeanSystem.LeanState.Center;
        }
    }
}