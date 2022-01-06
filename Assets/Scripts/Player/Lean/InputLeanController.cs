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
            if (Input.GetKeyDown(controls.leanLeft))
            {
                if (system.IsLeaning)
                    system.ResetLean();
                
                else system.LeanLeft();
            }

            if (Input.GetKeyDown(controls.leanRight))
            {
                if (system.IsLeaning)
                    system.ResetLean();
                
                else system.LeanRight();
            }
        }

        private void HandleHold()
        {
            if (Input.GetKeyDown(controls.leanLeft))
                system.LeanLeft();
            
            if (Input.GetKeyDown(controls.leanRight))
                system.LeanRight();
            
            if (system.IsLeaning && (Input.GetKeyUp(controls.leanLeft) || Input.GetKeyUp(controls.leanRight)))
                system.ResetLean();
        }
    }
}