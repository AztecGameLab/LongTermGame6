using UnityEngine;

[CreateAssetMenu]
public class SourceMovementStrategy : MovementStrategy
{
    [Header("Settings")]
    [SerializeField] public float noFrictionJumpWindow = 0.1f;
    [SerializeField] public float friction = 5.5f;
    [SerializeField] public float airAcceleration = 40f;
    [SerializeField] public float groundAcceleration = 50f;
    [SerializeField] public float maxAirSpeed = 1f;

    private Vector3 _velocity;
    
    public override Vector3 CalculateVelocity()
    {
        _velocity = MovementSystem.Velocity;
        
        return MovementSystem.IsGrounded
            ? MoveGround()
            : MoveAir();
    }
    
    private Vector3 MoveGround()
    {
        float speed = MovementSystem.GetCurrentSpeed();

        if (speed != 0 && MovementSystem.TimeSpentOnGround > noFrictionJumpWindow)
        {
            float drop = speed * friction * Time.deltaTime;
            _velocity *= Mathf.Max(speed - drop, 0) / speed;
        }

        return Accelerate(groundAcceleration, MovementSystem.CurrentMaxSpeed);
    }

    private Vector3 MoveAir()
    {
        return Accelerate(airAcceleration, maxAirSpeed);
    }
    
    private Vector3 Accelerate(float acceleration, float maxVelocity)
    {
        float projVel = Vector3.Dot(_velocity, MovementSystem.MovementDirection.normalized);
        float accelVel = acceleration * Time.deltaTime;

        if (projVel + accelVel > maxVelocity)
            accelVel = Mathf.Max(maxVelocity - projVel, 0);

        return _velocity + MovementSystem.MovementDirection * accelVel;
    }
}