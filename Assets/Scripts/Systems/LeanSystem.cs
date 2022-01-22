using UnityEngine;

namespace Player.Lean
{
    /// <summary>
    /// Allows an object to safely lean left and right, without clipping into a wall.
    /// </summary>
    
    public class LeanSystem : MyNamespace.System
    {
        [Header("Dependencies")]
        
        [SerializeField] 
        [Tooltip("The component that will animate our lean effect.")]
        private Animator leanAnimator;
        
        [SerializeField] 
        [Tooltip("The collider that is enabled when leaning left.")]
        private Collider leanLeftCollider;
        
        [SerializeField] 
        [Tooltip("The collider that is enabled when leaning right.")]
        private Collider leanRightCollider;

        // Internal State
        
        private static readonly int LeftTrigger = Animator.StringToHash("leanLeft");
        private static readonly int RightTrigger = Animator.StringToHash("leanRight");
        private static readonly int ResetLeanTrigger = Animator.StringToHash("resetLean");

        public bool IsLeaning { get; private set; }
        
        // Methods
        
        public void LeanLeft()
        {
            leanLeftCollider.enabled = true;
            leanRightCollider.enabled = false;
            
            IsLeaning = true;
            leanAnimator.SetTrigger(LeftTrigger);
        }

        public void LeanRight()
        {
            leanLeftCollider.enabled = false;
            leanRightCollider.enabled = true;
            
            IsLeaning = true;
            leanAnimator.SetTrigger(RightTrigger);
        }

        public void ResetLean()
        {
            leanLeftCollider.enabled = false;
            leanRightCollider.enabled = false;
            
            IsLeaning = false;
            leanAnimator.SetTrigger(ResetLeanTrigger);
        }
    }
}