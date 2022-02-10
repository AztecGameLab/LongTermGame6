using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Allows an object to switch between colliders in a safe way, without clipping into walls.
/// </summary>

public class CrouchSystem : MyNamespace.System
{
    [Header("Dependencies")] 
    [SerializeField]
    private GroundCheck groundCheck;
    
    [SerializeField] 
    [Tooltip("The trigger used to check if we can stand up without hitting our head.")]
    private Trigger standingTrigger;
    
    [SerializeField] 
    [Tooltip("The collider that is enabled when we are crouching.")]
    private Collider crouchCollider;
    
    [SerializeField] 
    [Tooltip("The collider that is enabled when we are standing.")]
    private Collider standingCollider;

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
        return WantsToCrouch && isGrounded && !_wasCrouching;
    }
    
    private void Crouch()
    {
        onCrouchStart.Invoke();

        crouchCollider.gameObject.SetActive(true);
        standingCollider.gameObject.SetActive(false);
    }

    private bool ShouldStand()
    {
        bool blockedAbove = standingTrigger.Colliders.Count > 0;
        bool isGrounded = groundCheck.IsGrounded;
        return !WantsToCrouch && isGrounded && !blockedAbove && _wasCrouching;
    }

    private void Stand()
    {
        onCrouchEnd.Invoke();
        
        crouchCollider.gameObject.SetActive(false);
        standingCollider.gameObject.SetActive(true);
    }
}