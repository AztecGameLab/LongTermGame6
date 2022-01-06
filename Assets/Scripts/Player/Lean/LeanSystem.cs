using UnityEngine;

namespace Player.Lean
{
    public class LeanSystem : MyNamespace.System
    {
        [SerializeField] private Animator leanAnimator;
        
        private static readonly int LeftTrigger = Animator.StringToHash("leanLeft");
        private static readonly int RightTrigger = Animator.StringToHash("leanRight");
        private static readonly int ResetLeanTrigger = Animator.StringToHash("resetLean");

        public bool IsLeaning { get; private set; }
        
        public void LeanLeft()
        {
            IsLeaning = true;
            leanAnimator.SetTrigger(LeftTrigger);
        }

        public void LeanRight()
        {
            IsLeaning = true;
            leanAnimator.SetTrigger(RightTrigger);
        }

        public void ResetLean()
        {
            IsLeaning = false;
            leanAnimator.SetTrigger(ResetLeanTrigger);
        }
    }
}