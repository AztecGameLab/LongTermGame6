using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Allows a Rigidbody to use advanced jumping mechanics like coyote time and air jumping.
/// </summary>

public class JumpingSystem : MyNamespace.System
{
    [Header("Settings")]
    
    [SerializeField] 
    private JumpSettings jumpSettings;
    
    [SerializeField]
    [Tooltip("Writes debugging info to the console.")]
    private bool showDebug;
    
    [Header("Dependencies")] 
    
    [SerializeField] 
    [Tooltip("The rigidbody to apply our jumping force to.")]
    private Rigidbody targetRigidbody;
    
    [SerializeField] 
    [Tooltip("Used to determine if we are grounded.")]
    private GroundCheck groundCheck;
    
    [SerializeField]
    [Tooltip("Controlled at runtime for special jumping effects, like holding to jump longer.")]
    private CustomGravity customGravity;
    
    [Space(20)]
    
    [SerializeField] 
    [Tooltip("Invoked when we jump.")]
    public UnityEvent onJump;
    
    // Internal State
    
    private bool _coyoteAvailable;
    private int _remainingAirJumps;

    private bool CoyoteAvailable => _coyoteAvailable && groundCheck.TimeSpentFalling < jumpSettings.CoyoteTime;
    public JumpSettings JumpSettings => jumpSettings;
    
    private bool _isInstanceNotNull;

    private void Start()
    {
        _isInstanceNotNull = HearingManager.Instance != null;
    }
    
    // Methods
    
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
        {
            ApplyJump();
            
            if(_isInstanceNotNull)
                HearingManager.Instance.OnSoundEmitted(gameObject, transform.position, EHeardSoundCategory.EJump, .5f);
        }
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