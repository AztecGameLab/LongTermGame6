using UnityEngine;

namespace Player.Lean
{
    public class LeanSystem : MyNamespace.System
    {
        [SerializeField] private Animator leanAnimator;
        [SerializeField] private Collider leanLeftCollider;
        [SerializeField] private Collider leanRightCollider;
        
        private static readonly int LeftTrigger = Animator.StringToHash("leanLeft");
        private static readonly int RightTrigger = Animator.StringToHash("leanRight");
        private static readonly int ResetLeanTrigger = Animator.StringToHash("resetLean");

        public bool IsLeaning { get; private set; }
        
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