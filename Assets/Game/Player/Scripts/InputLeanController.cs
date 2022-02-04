using System;
using UnityEngine;

namespace Player.Lean
{
    /// <summary>
    /// Controls the leaning system with input.
    /// </summary>
    
    // todo: the state logic is a bit confusing, and the LeanMode.Hold setting has not been tested very much.
    
    public class InputLeanController : InputController<LeanSystem>
    {
        public enum LeanMode
        {
            Hold,
            Toggle
        }

        [Header("Settings")]
        
        [SerializeField] 
        [Tooltip("Hold will only lean while the button is held. Toggle will remain leaning until pressed again.")]
        private LeanMode mode;
        
        [Header("Dependencies")]
        
        [SerializeField] 
        [Tooltip("Detects potential collisions when leaning left.")]
        private Trigger leanLeftTrigger;
        
        [SerializeField] 
        [Tooltip("Detects potential collisions when leaning right.")]
        private Trigger leanRightTrigger;
        
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
            if (Input.GetKeyDown(controls.leanLeft) && leanLeftTrigger.Colliders.Count == 0)
            {
                if (system.IsLeaning)
                    system.ResetLean();
                
                else system.LeanLeft();
            }

            if (Input.GetKeyDown(controls.leanRight) && leanRightTrigger.Colliders.Count == 0)
            {
                if (system.IsLeaning)
                    system.ResetLean();
                
                else system.LeanRight();
            }
        }

        private void HandleHold()
        {
            if (Input.GetKeyDown(controls.leanLeft) && leanLeftTrigger.Colliders.Count == 0)
                system.LeanLeft();
            
            if (Input.GetKeyDown(controls.leanRight) && leanRightTrigger.Colliders.Count == 0)
                system.LeanRight();
            
            if (system.IsLeaning && (Input.GetKeyUp(controls.leanLeft) || Input.GetKeyUp(controls.leanRight)))
                system.ResetLean();
        }
    }
}