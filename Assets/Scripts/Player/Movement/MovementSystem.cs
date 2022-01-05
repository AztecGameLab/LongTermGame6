using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Moves a Rigidbody in a direction.
/// </summary>

public class MovementSystem : MyNamespace.System
{
    [Header("Dependencies")] 
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private Transform lookDirection;
    
    [Header("Settings")]
    [SerializeField] private MovementSettings movementSettings;

    private float _speedMultiplier = 1f;
    private float _forwardSpeedMultiplier = 1f;
    
    [PublicAPI] public float SpeedMultiplier
    {
        get => _speedMultiplier;
        set => _speedMultiplier *= value;
    }
    
    [PublicAPI] public float ForwardSpeedMultiplier
    {
        get => _forwardSpeedMultiplier;
        set => _forwardSpeedMultiplier *= value;
    }

    [PublicAPI] public float CurrentRunningSpeed
    {
        get
        {
            Vector3 velocity = targetRigidbody.velocity;
            velocity.y = 0;
            return velocity.magnitude;
        }
    }
    [PublicAPI] public float CurrentMaxSpeed => movementSettings.MovementSpeed * SpeedMultiplier;
    [PublicAPI] public Vector3 MovementDirection { get; set; } = Vector3.zero;
    [PublicAPI] public Rigidbody Rigidbody => targetRigidbody;
    [PublicAPI] public GroundCheck GroundCheck => groundCheck;
    
    public void UpdateMovement(Vector3 direction)
    {
        MovementDirection = direction;
        
        ApplyForwardSpeedMultiplier();
        
        targetRigidbody.velocity = movementSettings.UpdateVelocity(this);
    }

    private void ApplyForwardSpeedMultiplier()
    {
        float forwardSpeed = Vector3.Dot(MovementDirection, lookDirection.forward);
        
        if (forwardSpeed > 0.1f)
            MovementDirection += lookDirection.forward * forwardSpeed * (ForwardSpeedMultiplier - 1);
    }
}