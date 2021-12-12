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

    private MovementSystem _movementSystem;

    private bool IsAccelerating => _movementSystem.Rigidbody.velocity != Vector3.zero;
    public float MovementSpeed => movementSpeed;
    
    [PublicAPI] 
    public Vector3 UpdateVelocity(MovementSystem system)
    {
        _movementSystem = system;

        Vector3 velocity = CalculateTargetVelocity();
        float acceleration = CalculateAcceleration();

        return Vector3.MoveTowards(system.Rigidbody.velocity, velocity, acceleration);
    }

    private Vector3 CalculateTargetVelocity()
    {
        Vector3 direction = _movementSystem.MovementDirection;
        float speed = _movementSystem.CurrentMaxSpeed;
        
        Vector3 velocity = direction * speed;
        velocity.y = _movementSystem.Rigidbody.velocity.y;
        
        return velocity;
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