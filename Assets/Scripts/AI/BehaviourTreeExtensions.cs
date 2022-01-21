using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace UnityTemplateProjects
{
    public static class BehaviourTreeExtensions
    {
        private static MoveTowardsTask _moveTowardsTask = new MoveTowardsTask();
        private static LookAtTask _lookAtTask = new LookAtTask();
        
        /// <summary>
        /// Move this agent to a position in the world.
        /// </summary>
        /// <param name="agent">The agent to move.</param>
        /// <param name="position">The position we want to move to.</param>
        /// <param name="stopDistance">How close we need to be in order to reach our destination.</param>
        /// <returns>Succeeds when we reach our destination. Continues when traveling. Fails if no path is possible.</returns>
        public static TaskStatus MoveTowards(this NavMeshAgent agent, Vector3 position, float stopDistance = 0.1f)
        {
             return _moveTowardsTask.Perform(agent, position, stopDistance);
        }
        
        /// <summary>
        /// Rotate this system to look at a position in the world.
        /// </summary>
        /// <param name="rotationSystem">The system to rotate.</param>
        /// <param name="position">The position to look at.</param>
        /// <param name="smoothTime">The duration of the rotation (0 for no smoothing).</param>
        /// <returns>Succeeds when we are looking at the position. Continues when moving. Will not fail.</returns>
        public static TaskStatus LookAt(this RotationSystem rotationSystem, Vector3 position, float smoothTime = 0.25f)
        {
            return _lookAtTask.Perform(rotationSystem, position, smoothTime);
        }
    }
}