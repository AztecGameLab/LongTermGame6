using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Moves a Rigidbody in a direction.
/// </summary>

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private MovementSettings movementSettings;
    [SerializeField] private bool showDebug;

    [Header("Dependencies")] 
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private GroundedCheck groundCheck;
    [SerializeField] private Transform lookDirection;

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

    [PublicAPI] public float CurrentMaxSpeed => movementSettings.MovementSpeed * SpeedMultiplier;
    [PublicAPI] public Vector3 MovementDirection { get; set; } = Vector3.zero;
    [PublicAPI] public Rigidbody Rigidbody => targetRigidbody;
    [PublicAPI] public GroundedCheck GroundCheck => groundCheck;
    
    private void Update()
    {
        ApplyForwardSpeedMultiplier();
        
        targetRigidbody.velocity = movementSettings.UpdateVelocity(this);
    }

    private void ApplyForwardSpeedMultiplier()
    {
        float forwardSpeed = Vector3.Dot(MovementDirection, lookDirection.forward);
        
        if (forwardSpeed > 0.1f)
            MovementDirection += lookDirection.forward * forwardSpeed * (ForwardSpeedMultiplier - 1);
    }

    #region Debug

    private void OnGUI()
    {
        if (showDebug)
            DrawDebugUI();
    }

    private void DrawDebugUI()
    {
        Vector3 velocity = targetRigidbody.velocity;
        velocity.y = 0;
        float currentRunningSpeed = velocity.magnitude;
        
        GUILayout.Label($"Movement Direction: {MovementDirection}");
        GUILayout.Label($"Current Speed (Max {CurrentMaxSpeed * ForwardSpeedMultiplier}): {currentRunningSpeed}");
        GUILayout.Label($"Forward Direction Multiplier: {ForwardSpeedMultiplier}");
    }    

    #endregion
}