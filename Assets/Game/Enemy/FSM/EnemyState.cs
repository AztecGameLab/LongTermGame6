using UnityEngine;

namespace Game.Enemy
{
    public abstract class EnemyState : MonoBehaviour
    {
        public abstract string StateName { get; }
        
        public virtual void OnStateEnter(EnemyStateManager enemy) { }
        public virtual void OnStateExit(EnemyStateManager enemy) { }
        public virtual void OnStateUpdate(EnemyStateManager enemy) { }
    }
}