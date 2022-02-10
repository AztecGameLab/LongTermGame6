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
        
        [SerializeField] private Transform rightTransform;
        [SerializeField] private Transform centerTransform;
        [SerializeField] private Transform leftTransform;
        
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
        
        public LeanState TargetState { get; set; }
        public LeanState CurrentState { get; private set; }
        
        // Methods

        private void Update()
        {
            switch (TargetState)
            {
                case LeanState.Left  : UpdateLean(leftCollider, leftTransform);
                    break;
                case LeanState.Right : UpdateLean(rightCollider, rightTransform);
                    break;
                case LeanState.Center: UpdateLean(centerCollider, centerTransform);
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private float _startTime;
        
        private void UpdateLean(SafeCollider targetCollider, Transform targetTransform)
        {
            // We can only animate our leaning if there is nothing in our way.
            if (targetCollider.IsClear)
            {
                // On the first frame that we change state, do some initialization. 
                if (CurrentState != TargetState)
                {
                    CurrentState = TargetState;
                    _startTime = Time.time;

                    leftCollider.Disable();
                    rightCollider.Disable();
                    centerCollider.Disable();
                    targetCollider.SafeEnable();
                }

                // Animate our transform's position and rotation with the SmoothStep function.
                // See: https://en.wikipedia.org/wiki/Smoothstep
                
                float percentDone = math.smoothstep(0, leanDuration, Time.time - _startTime);
                leanTransform.position = Vector3.Lerp(leanTransform.position, targetTransform.position, percentDone);
                leanTransform.localRotation = Quaternion.Lerp(leanTransform.localRotation, targetTransform.localRotation, percentDone);
            }
        }

        private void OnGUI()
        {
            GUILayout.Label($"Target State {TargetState}");
            GUILayout.Label($"Current State {CurrentState}");
        }
    }
}