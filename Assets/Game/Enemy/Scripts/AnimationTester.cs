using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class AnimationTester : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        
        [Range(0, 1)]
        public float speed;

        private Vector3 previousPosition;
        
        [Button]
        public void Attack()
        {
            animator.SetTrigger("Attack");
        }

        private float _speed;
        
        private void Update()
        {
            Vector3 velocity = previousPosition - transform.position;
            _speed = velocity.magnitude / Time.deltaTime;
            
            animator.SetFloat("MovementSpeed", _speed);

            previousPosition = transform.position;
        }
    }
}