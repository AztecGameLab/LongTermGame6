using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Allows a Rigidbody to use advanced jumping mechanics like coyote time and air jumping.
/// </summary>

public class JumpingSystem : MyNamespace.System
{
    [Header("Dependencies")] 
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private CustomGravity customGravity;
    
    [Header("Settings")]
    [SerializeField] private JumpSettings jumpSettings;
    [SerializeField] private bool showDebug;

    [Space(20)]
    [SerializeField] public UnityEvent onJump;
    
    private bool _coyoteAvailable;
    private int _remainingAirJumps;

    private bool CoyoteAvailable => _coyoteAvailable && groundCheck.TimeSpentFalling < jumpSettings.CoyoteTime;
    public JumpSettings JumpSettings => jumpSettings;
    
    [PublicAPI] 
    public void RefreshJumps()
    {
        if (showDebug) 
            Debug.Log("Jumps were refreshed!");
        
        _remainingAirJumps = jumpSettings.AirJumps;
        _coyoteAvailable = true;
    }

    public void TryToJump()
    {
        if (ShouldJump())
            ApplyJump();
    }

    private bool ShouldJump()
    {
        if (groundCheck.IsGrounded || CoyoteAvailable)
        {
            if (showDebug) 
                Debug.Log(groundCheck.IsGrounded ? "Jumped: Normal" : "Jumped: Coyote");
            
            return true;
        }

        if (!groundCheck.IsGrounded && _remainingAirJumps > 0)
        {
            if (showDebug) 
                Debug.Log("Jumped: Air");
            
            _remainingAirJumps--;
            return true;
        }

        return false;
    }

    private void ApplyJump()
    {
        Vector3 currentVelocity = targetRigidbody.velocity;
        currentVelocity.y = jumpSettings.JumpSpeed;
        targetRigidbody.velocity = currentVelocity;
        
        _coyoteAvailable = false;
        onJump.Invoke();
    }

    public void UpdateGravity(bool holdingJump)
    {
        bool rising = targetRigidbody.velocity.y > 0;
        float currentGravity = jumpSettings.GetCurrentGravity(rising, holdingJump);

        customGravity.gravityMagnitude = currentGravity;
    }
}