using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace UnityTemplateProjects
{
    public static class BehaviourTreeExtensions
    {
        private static MoveTowardsTask _moveTowardsTask = new MoveTowardsTask();
        private static LookAtTask _lookAtTask = new LookAtTask();
        
        public static TaskStatus MoveTowards(this NavMeshAgent agent, Vector3 position, float stopDistance = 0.1f)
        {
             return _moveTowardsTask.Perform(agent, position, stopDistance);
        }
        
        public static TaskStatus LookAt(this RotationSystem rotationSystem, Vector3 position, float smoothTime = 0.25f)
        {
            return _lookAtTask.Perform(rotationSystem, position, smoothTime);
        }
    }

    public class MoveTowardsTask
    {
        private NavMeshAgent _agent;
        private Vector3 _target;
        private float _stopDistance;

        public TaskStatus Perform(NavMeshAgent agent, Vector3 target, float stopDistance)
        {
            _agent = agent;
            _target = target;
            _stopDistance = stopDistance;
            
            if (_agent == null)
                return TaskStatus.Failure;

            return ApplyMovement();
        }

        private TaskStatus ApplyMovement()
        {
            if (_agent.pathPending)
                return TaskStatus.Continue;
            
            if (CloseToTarget())
            {
                _agent.ResetPath();
                return TaskStatus.Success;
            }
            
            _agent.SetDestination(_target);

            return TaskStatus.Continue;
        }
        
        private bool CloseToTarget()
        {
            bool hasPath = _agent.hasPath;
            bool isNearby = _agent.remainingDistance < _stopDistance;
            
            return hasPath && isNearby;
        }
    }

    public class LookAtTask
    {
        private RotationSystem _rotationSystem;
        private Vector3 _target;
        private float _smoothTime;
        private Vector3 _velocity;

        public TaskStatus Perform(RotationSystem rotationSystem, Vector3 target, float smoothTime)
        {
            _rotationSystem = rotationSystem;
            _smoothTime = smoothTime;
            _target = target;
            
            if (_target == null)
                return TaskStatus.Failure;

            return ApplyRotation();
        }

        private TaskStatus ApplyRotation()
        {
            Vector3 currentView = _rotationSystem.Forward;
            Vector3 targetView = (_target - _rotationSystem.PitchTransform.position).normalized;
            bool hasReachedTarget = (currentView - targetView).magnitude <= 0.1f;

            if (hasReachedTarget)
                return TaskStatus.Success;
            
            _rotationSystem.Forward = Vector3.SmoothDamp(currentView, targetView, ref _velocity, _smoothTime);
            return TaskStatus.Continue;
        }
    }
}