using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu]
public class MovementSettings : ScriptableObject 
{
    [Header("General Settings")]
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float groundedAcceleration = 0.125f;
    [SerializeField] private float groundedDeceleration = 0.2f;
    [SerializeField] private float airborneAcceleration = 0.6f;
    [SerializeField] private float airborneDeceleration = 0.6f;
    [SerializeField] private float decelerationRampPercent = 0.5f;
    
    private MovementSystem _movementSystem;
    
    private bool IsAccelerating => _movementSystem.MovementDirection.x != 0 || _movementSystem.MovementDirection.z != 0;
    public float MovementSpeed => movementSpeed;
    
    [PublicAPI] 
    public Vector3 UpdateVelocity(MovementSystem system)
    {
        _movementSystem = system;

        if (IsAccelerating || system.GroundCheck.IsGrounded == false)
            return CalculateConstantAcceleration();

        return CalculateSmoothStop();
    }

    private Vector3 CalculateConstantAcceleration()
    {
        Vector3 currentVelocity = _movementSystem.Rigidbody.velocity;
        Vector3 targetVelocity = CalculateTargetVelocity();
        float acceleration = CalculateAcceleration();

        return Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration);
    }
    
    private Vector3 CalculateSmoothStop()
    {
        Vector3 originalVelocity = _movementSystem.Rigidbody.velocity;
        Vector3 smoothedVelocity = originalVelocity;

        float deceleration = decelerationRampPercent * Time.deltaTime;
        smoothedVelocity.x -= smoothedVelocity.x * deceleration;
        smoothedVelocity.z -= smoothedVelocity.z * deceleration;

        return smoothedVelocity;
    }

    private Vector3 CalculateTargetVelocity()
    {
        Vector3 direction = _movementSystem.MovementDirection;
        float speed = _movementSystem.CurrentMaxSpeed;

        return new Vector3(direction.x * speed, _movementSystem.Rigidbody.velocity.y, direction.z * speed);
    }

    private float CalculateAcceleration()
    {
        float currentGroundedAcceleration = IsAccelerating
            ? GetAccelerationDelta(groundedAcceleration)
            : GetAccelerationDelta(groundedDeceleration); 
        
        float currentAirborneAcceleration = IsAccelerating 
            ? GetAccelerationDelta(airborneAcceleration) 
            : GetAccelerationDelta(airborneDeceleration);

        return _movementSystem.GroundCheck.IsGrounded ? currentGroundedAcceleration : currentAirborneAcceleration;
    }
    
    private float GetAccelerationDelta(float acceleration)
    {
        return 1 / acceleration * _movementSystem.CurrentMaxSpeed * Time.deltaTime;
    }
}