using UnityEngine;

public abstract class MovementStrategy : ScriptableObject
{
    protected MovementSystem MovementSystem;
    
    public void Initialize(MovementSystem movementSystem)
    {
        MovementSystem = movementSystem;
    }
    
    public abstract Vector3 CalculateVelocity();
}