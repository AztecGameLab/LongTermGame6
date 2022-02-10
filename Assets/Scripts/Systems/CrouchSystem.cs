using UnityEngine;
using UnityEngine.Events;
using Utility;

/// <summary>
/// Allows an object to switch between colliders in a safe way, without clipping into walls.
/// </summary>

public class CrouchSystem : MyNamespace.System
{
    [Header("Dependencies")] 
    [SerializeField]
    private GroundCheck groundCheck;
    
    [SerializeField] 
    [Tooltip("The collider that is enabled when we are crouching.")]
    private SafeCollider crouchCollider;
    
    [SerializeField] 
    [Tooltip("The collider that is enabled when we are standing.")]
    private SafeCollider standingCollider;

    [Space(20)]
    
    [SerializeField] 
    [Tooltip("Called when we start crouching.")]
    private UnityEvent onCrouchStart;
    
    [SerializeField] 
    [Tooltip("Called when we stop crouching.")]
    private UnityEvent onCrouchEnd;

    // Internal State
    
    public bool WantsToCrouch { get; set; }
    
    private bool _isCrouching;
    private bool _wasCrouching;

    // Methods
    
    private void Update()
    {
        _wasCrouching = _isCrouching;

        if (ShouldCrouch())
        {
            _isCrouching = true;
            Crouch();
        }
        
        else if (ShouldStand())
        {
            _isCrouching = false;
            Stand();
        }
    }

    private bool ShouldCrouch()
    {
        bool isGrounded = groundCheck.IsGrounded;
        return WantsToCrouch && isGrounded && !_wasCrouching && crouchCollider.IsClear;
    }
    
    private void Crouch()
    {
        onCrouchStart.Invoke();

        crouchCollider.SafeEnable();
        standingCollider.Disable();
    }

    private bool ShouldStand()
    {
        bool isGrounded = groundCheck.IsGrounded;
        return !WantsToCrouch && isGrounded && _wasCrouching && standingCollider.IsClear;
    }

    private void Stand()
    {
        onCrouchEnd.Invoke();
        
        crouchCollider.Disable();
        standingCollider.SafeEnable();
    }
}