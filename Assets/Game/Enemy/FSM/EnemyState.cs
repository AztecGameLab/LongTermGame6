using UnityEngine;
using Console = ConsoleUtility.Console;

namespace Game.Enemy
{
    public abstract class EnemyState : MonoBehaviour
    {
        public abstract string StateName { get; }
        
        public virtual void OnStateEnter(EnemyStateManager enemy)
        {
            if (enemy.ShowDebug)
                Console.Log($"Entered: {StateName}");
        }
        
        public virtual void OnStateExit(EnemyStateManager enemy)
        {
            if (enemy.ShowDebug)
                Console.Log($"Exited: {StateName}");
        }

        public virtual void OnStateUpdate(EnemyStateManager enemy)
        {
            // Probably don't want to log every frame.
        }
    }
}