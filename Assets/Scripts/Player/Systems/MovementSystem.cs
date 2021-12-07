using JetBrains.Annotations;
using UnityEngine;

// todo: cleanup

public class MovementSystem : MonoBehaviour
{
    [SerializeField] protected bool showDebug;

    [Header("Dependencies")] 
    [SerializeField] private MovementSettings settings;
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private GroundedCheck groundCheck;
    [SerializeField] private Transform lookDirection;

    [PublicAPI] public bool IsGrounded => groundCheck.IsGrounded;
    [PublicAPI] public float BaseMovementSpeed => settings.baseMovementSpeed;
    [PublicAPI] public float TimeSpentOnGround => groundCheck.TimeSpentGrounded;
    [PublicAPI] public float CurrentMaxSpeed { get; set; }
    [PublicAPI] public float ForwardSpeedMultiplier { get; set; } = 1;
    [PublicAPI] public Vector3 MovementDirection { get; set; }
    [PublicAPI] public Vector3 Velocity => targetRigidbody.velocity;
    
    private void Start()      { SetupVariables(); }
    private void OnValidate() { SetupVariables(); }

    private void SetupVariables()
    {
        CurrentMaxSpeed = settings.baseMovementSpeed;
    }

    public float GetCurrentSpeed()
    {
        Vector3 velocity = Velocity;
        velocity.y = 0;

        return velocity.magnitude;
    }
    
    private void Update()
    {
        ApplyForwardSpeedMultiplier();
        Vector3 targetVelocity = CustomUpdateVelocity();
        ApplyVelocity(targetVelocity); 
    }

    private void ApplyForwardSpeedMultiplier()
    {
        float forwardSpeed = Vector3.Dot(MovementDirection, lookDirection.forward);
        
        if (forwardSpeed > 0.1f)
            MovementDirection += lookDirection.forward * forwardSpeed * (ForwardSpeedMultiplier - 1);
    }

    private Vector3 CustomUpdateVelocity()
    {
        Vector3 targetVelocity = MovementDirection * CurrentMaxSpeed;
        targetVelocity.y = Velocity.y;

        float currentGroundedAcceleration = IsAccelerating(targetVelocity)
            ? CalculateAcceleration(settings.groundedAcceleration)
            : CalculateAcceleration(settings.groundedDeceleration); 
        
        float currentAirborneAcceleration = IsAccelerating(targetVelocity) 
            ? CalculateAcceleration(settings.airborneAcceleration) 
            : CalculateAcceleration(settings.airborneDeceleration);

        float acceleration = IsGrounded ? currentGroundedAcceleration : currentAirborneAcceleration;
        
        return Vector3.MoveTowards(Velocity, targetVelocity, acceleration);
    }

    private void ApplyVelocity(Vector3 targetVelocity)
    {
        targetRigidbody.velocity = targetVelocity; 
    }
    
    private float CalculateAcceleration(float acceleration)
    {
        return 1 / acceleration * CurrentMaxSpeed * Time.deltaTime;
    }

    private static bool IsAccelerating(Vector3 targetVelocity)
    {
        return targetVelocity != Vector3.zero;
    }

    private void OnGUI()
    {
        if (showDebug)
            DrawDebugUI();
    }

    private void DrawDebugUI()
    {
        GUILayout.Label($"Movement Direction: {MovementDirection}");
        GUILayout.Label($"Current Speed: {GetCurrentSpeed()}");
        GUILayout.Label($"Current Velocity: {Velocity}");
        GUILayout.Label($"Current Max Speed: {CurrentMaxSpeed}");
        GUILayout.Label($"Forward Direction Multiplier: {ForwardSpeedMultiplier}");
    }
}