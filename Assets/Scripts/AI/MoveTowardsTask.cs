using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace UnityTemplateProjects
{
    // todo: "Succeeds when we reach our destination. Continues when traveling. Fails if no path is possible." ensure this is correct behaviour, and clean up
    // todo: consider better naming
    
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
}