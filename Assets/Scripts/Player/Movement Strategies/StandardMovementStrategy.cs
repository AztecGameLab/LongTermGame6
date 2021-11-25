using UnityEngine;

[CreateAssetMenu]
public class StandardMovementStrategy : MovementStrategy
{
    [Header("Settings")]
    [SerializeField] public float groundedAcceleration = 0.125f;
    [SerializeField] public float groundedDeceleration = 0.125f;
    [SerializeField] public float airborneAcceleration = 0.3f;
    [SerializeField] public float airborneDeceleration = 0.3f;

    public override Vector3 CalculateVelocity()
    {
        Vector3 targetVelocity = MovementSystem.MovementDirection * MovementSystem.CurrentMaxSpeed;
        targetVelocity.y = MovementSystem.Velocity.y;

        float currentGroundedAcceleration = IsAccelerating(targetVelocity)
            ? CalculateAcceleration(groundedAcceleration)
            : CalculateAcceleration(groundedDeceleration); 
        
        float currentAirborneAcceleration = IsAccelerating(targetVelocity) 
            ? CalculateAcceleration(airborneAcceleration) 
            : CalculateAcceleration(airborneDeceleration);

        float acceleration = MovementSystem.IsGrounded ? currentGroundedAcceleration : currentAirborneAcceleration;
        
        return Vector3.MoveTowards(MovementSystem.Velocity, targetVelocity, acceleration);
    }

    private float CalculateAcceleration(float acceleration)
    {
        return 1 / acceleration * MovementSystem.CurrentMaxSpeed * Time.deltaTime;
    }

    private static bool IsAccelerating(Vector3 targetVelocity)
    {
        return targetVelocity != Vector3.zero;
    }
}