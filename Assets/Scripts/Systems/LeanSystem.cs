using System;
using Unity.Mathematics;
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

        [Header("Settings")] 
        [SerializeField] private Transform leanTransform;
        [SerializeField] private float leanDuration = 1f;
        
        [SerializeField] private Transform rightTarget;
        [SerializeField] private Transform leftTarget;
        [SerializeField] private Transform centerTarget;
        
        [Header("Dependencies")]

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
        
        private float _leanStartTime;

        public LeanState TargetState { get; set; } = LeanState.Center;
        public LeanState CurrentState { get; private set; } = LeanState.Center;
        
        // Methods

        private void Update()
        {
            switch (TargetState)
            {
                case LeanState.Left: 
                    if (leftCollider.IsClear)
                        UpdateLean(leftCollider, leftTarget);
                    break;
                
                case LeanState.Right:
                    if (rightCollider.IsClear)
                        UpdateLean(rightCollider, rightTarget);
                    break;
                
                case LeanState.Center:
                    if (centerCollider.IsClear)
                        UpdateLean(centerCollider, centerTarget);
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }

        
        private void UpdateLean(SafeCollider targetCollider, Transform targetTransform)
        {
            if (CurrentState != TargetState)
            {
                leftCollider.Disable();
                rightCollider.Disable();
                centerCollider.Disable();
                
                targetCollider.SafeEnable();
                
                _leanStartTime = Time.time;
                CurrentState = TargetState;
            }

            ApplyAnimation(targetTransform.position, targetTransform.localRotation);
        }

        // Animate our transform's position and rotation with the SmoothStep function.
        // See: https://en.wikipedia.org/wiki/Smoothstep
        
        private void ApplyAnimation(Vector3 targetPosition, Quaternion targetRotation)
        {
            float elapsedTime = Time.time - _leanStartTime;
            float percentDone = math.smoothstep(0, leanDuration, elapsedTime * Time.timeScale);
        
            leanTransform.position = Vector3.Lerp(leanTransform.position, targetPosition, percentDone);
            leanTransform.localRotation = Quaternion.Lerp(leanTransform.localRotation, targetRotation, percentDone);
        }
    }
}