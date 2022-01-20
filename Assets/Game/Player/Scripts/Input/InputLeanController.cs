using System;
using UnityEngine;

namespace Player.Lean
{
    public class InputLeanController : InputController<LeanSystem>
    {
        public enum LeanMode
        {
            Hold,
            Toggle
        }

        [SerializeField] private LeanMode mode;
        [SerializeField] private Trigger leanLeftTrigger;
        [SerializeField] private Trigger leanRightTrigger;
        
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
            if (Input.GetKeyDown(controls.leanLeft) && !leanLeftTrigger.IsOccupied)
            {
                if (system.IsLeaning)
                    system.ResetLean();
                
                else system.LeanLeft();
            }

            if (Input.GetKeyDown(controls.leanRight) && !leanRightTrigger.IsOccupied)
            {
                if (system.IsLeaning)
                    system.ResetLean();
                
                else system.LeanRight();
            }
        }

        private void HandleHold()
        {
            if (Input.GetKeyDown(controls.leanLeft) && !leanLeftTrigger.IsOccupied)
                system.LeanLeft();
            
            if (Input.GetKeyDown(controls.leanRight) && !leanRightTrigger.IsOccupied)
                system.LeanRight();
            
            if (system.IsLeaning && (Input.GetKeyUp(controls.leanLeft) || Input.GetKeyUp(controls.leanRight)))
                system.ResetLean();
        }
    }
}