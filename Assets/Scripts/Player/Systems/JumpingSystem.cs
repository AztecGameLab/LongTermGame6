using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Allows a Rigidbody to use advanced jumping mechanics like coyote time and air jumping.
/// </summary>

public class JumpingSystem : MonoBehaviour
{
    [SerializeField] private JumpSettings jumpSettings;
    [SerializeField] private bool showDebug;

    [Header("Dependencies")] 
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private GroundedCheck groundCheck;
    [SerializeField] private CustomGravity customGravity;

    [Space(20)]
    [SerializeField] public UnityEvent onJump;
    
    private bool _coyoteAvailable;
    private int _remainingAirJumps;

    private bool CoyoteAvailable => _coyoteAvailable && groundCheck.TimeSpentFalling < jumpSettings.CoyoteTime;
    public JumpSettings JumpSettings => jumpSettings;
    
    [PublicAPI] public bool HoldingJump { get; set; }
    [PublicAPI] public bool WantsToJump { get; set; }

    [PublicAPI] 
    public void RefreshJumps()
    {
        if (showDebug) 
            Debug.Log("Jumps were refreshed!");
        
        _remainingAirJumps = jumpSettings.AirJumps;
        _coyoteAvailable = true;
    }

    private void FixedUpdate()
    {
        if (ShouldJump())
            ApplyJump();

        ApplyGravity();
    }

    private bool ShouldJump()
    {
        if (WantsToJump == false)
            return false;

        if (groundCheck.IsGrounded || CoyoteAvailable)
        {
            if (showDebug) 
                Debug.Log(groundCheck.IsGrounded ? "Jumped: Normal" : "Jumped: Coyote");
            
            return true;
        }

        if (_remainingAirJumps > 0)
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
        targetRigidbody.velocity = targetRigidbody.velocity.SetY(jumpSettings.JumpSpeed);
        _coyoteAvailable = false;
        onJump.Invoke();
    }

    private void ApplyGravity()
    {
        bool rising = targetRigidbody.velocity.y > 0;
        float currentGravity = jumpSettings.GetCurrentGravity(rising, HoldingJump);

        customGravity.gravity = currentGravity;
    }

    #region Debug

    private void OnGUI()
    {
        if (showDebug)
            DrawDebugUI();
    }

    private void DrawDebugUI()
    {
        GUILayout.Label($"Holding Jump: {HoldingJump}");
        GUILayout.Label($"Wants to Jump: {WantsToJump}");
        GUILayout.Label($"Coyote Available: {CoyoteAvailable}");
        GUILayout.Label($"Remaining Air Jumps: {_remainingAirJumps}");
    }

    #endregion
}