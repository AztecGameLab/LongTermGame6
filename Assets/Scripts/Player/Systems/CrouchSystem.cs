using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

// todo: cleanup

public class CrouchSystem : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private bool showDebug;
    [SerializeField] [Range(0, 1)] private float crouchSpeedMultiplier = 0.25f;
    [SerializeField] private float radiusBuffer = 0.1f;
    [SerializeField] private float heightBuffer = 0.1f;

    [Header("Dependencies")] 
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private CapsuleCollider playerCollider;

    [Header("Events")] 
    [SerializeField] private UnityEvent onCrouchStart;
    [SerializeField] private UnityEvent onCrouchEnd;
    
    private bool _blockedAbove;
    private bool _wasCrouching;
    
    private bool CrouchJustStarted => IsCrouching && !_wasCrouching;
    private bool CrouchJustEnded => !IsCrouching && _wasCrouching;
    
    [PublicAPI] public bool WantsToCrouch { get; set; }
    [PublicAPI] public bool IsCrouching { get; private set; }
    
    private void Update()
    {
        _blockedAbove = CheckIfBlockedAbove();
        
        if (!_blockedAbove)
            IsCrouching = WantsToCrouch;
     
        UpdateSpeed();
        UpdateEvents();
        
        _wasCrouching = IsCrouching;
    }
    
    private void UpdateEvents()
    {
        if (CrouchJustStarted)
            onCrouchStart.Invoke();
        
        else if (CrouchJustEnded)
            onCrouchEnd.Invoke();
    }

    private bool CheckIfBlockedAbove()
    {
        var ray = new Ray(playerCollider.bounds.center, playerCollider.transform.up);
        float radius = playerCollider.radius - radiusBuffer;
        float maxDistance = playerCollider.height / 2 + heightBuffer;
        
        return Physics.SphereCast(ray, radius, maxDistance);
    }

    private void UpdateSpeed()
    {
        float baseMovementSpeed = movementSystem.BaseMovementSpeed;
        
        if (WantsToCrouch)
            movementSystem.CurrentMaxSpeed = baseMovementSpeed * crouchSpeedMultiplier;
        
        else if ((WantsToCrouch || _blockedAbove) == false)
            movementSystem.CurrentMaxSpeed = baseMovementSpeed;
    }

    private void OnGUI()
    {
        if (showDebug)
            DrawDebugUI();
    }

    private void DrawDebugUI()
    {
        GUILayout.Label($"Holding crouch: {WantsToCrouch}");
        GUILayout.Label($"Blocked above: {_blockedAbove}");
    }
}