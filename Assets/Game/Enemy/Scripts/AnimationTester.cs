using System;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Enemy
{
    public class AnimationTester : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        [Range(0, 1)]
        public float speed;

        [Button]
        public void Attack()
        {
            animator.SetTrigger("Attack");
        }

        private void Update()
        {
            // this is me from the browser !!!
            animator.SetFloat("MovementSpeed", speed);
        }
    }
}