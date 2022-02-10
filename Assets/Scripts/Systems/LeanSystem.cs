using System;
using UnityEngine;
using Utility;

namespace Player.Lean
{
    /// <summary>
    /// Allows an object to safely lean left and right, without clipping into a wall.
    /// </summary>
    
    public class LeanSystem : MyNamespace.System
    {
        public enum LeanState { Left, Right, Center }

        [Header("Dependencies")]
        
        [SerializeField] 
        [Tooltip("The component that will animate our lean effect.")]
        private Animator leanAnimator;
        
        [SerializeField] 
        [Tooltip("The collider that is enabled when leaning left.")]
        private SafeCollider leftCollider;
        
        [SerializeField] 
        [Tooltip("The collider that is enabled when leaning right.")]
        private SafeCollider rightCollider;
        
        [SerializeField] 
        [Tooltip("The collider that is enabled when leaning right.")]
        private SafeCollider centerCollider;

        // Internal State
        
        private static readonly int LeftTrigger = Animator.StringToHash("leanLeft");
        private static readonly int RightTrigger = Animator.StringToHash("leanRight");
        private static readonly int CenterTrigger = Animator.StringToHash("resetLean");
        
        public LeanState TargetState { get; set; }
        public LeanState CurrentState { get; private set; }
        
        // Methods

        private void Update()
        {
            switch (TargetState)
            {
                case LeanState.Left  : UpdateLean(leftCollider, LeftTrigger);
                    break;
                case LeanState.Right : UpdateLean(rightCollider, RightTrigger);
                    break;
                case LeanState.Center: UpdateLean(centerCollider, CenterTrigger);
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateLean(SafeCollider targetCollider, int animationTrigger)
        {
            if (targetCollider.IsClear && CurrentState != TargetState)
            {
                CurrentState = TargetState;
                
                leftCollider.Disable();
                rightCollider.Disable();
                centerCollider.Disable();
                
                targetCollider.SafeEnable();
                
                leanAnimator.SetTrigger(animationTrigger);
            }
        }
    }
}