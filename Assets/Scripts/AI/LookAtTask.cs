using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace UnityTemplateProjects
{
    // todo: consider better naming
    // todo: fix namespacing across the project
    
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