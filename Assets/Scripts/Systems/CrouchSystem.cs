using Unity.Mathematics;
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

    [SerializeField] 
    private Transform crouchTarget;
    
    [SerializeField]
    private Transform standingTarget;
    
    [SerializeField] private Transform crouchTransform;
    [SerializeField] private float crouchDuration = 1f;

    [Space(20)]
    
    [SerializeField] 
    [Tooltip("Called when we start crouching.")]
    private UnityEvent onCrouchStart;
    
    [SerializeField] 
    [Tooltip("Called when we stop crouching.")]
    private UnityEvent onCrouchEnd;

    // Internal State
    
    public bool WantsToCrouch { get; set; }

    public bool IsCrouching => _isCrouching;

    private bool _isCrouching;
    private bool _wasCrouching;
    private float _crouchStartTime;

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
        bool isNotObstructed = crouchCollider.IsClear;
        
        return WantsToCrouch && isGrounded && isNotObstructed;
    }
    
    private bool ShouldStand()
    {
        bool isGrounded = groundCheck.IsGrounded;
        bool isNotObstructed = standingCollider.IsClear;
        bool wantsToStand = !WantsToCrouch;
        
        return wantsToStand && isGrounded && isNotObstructed;
    }

    private void Crouch()
    {
        if (!_wasCrouching)
        {
            onCrouchStart.Invoke();
            crouchCollider.SafeEnable();
            standingCollider.Disable();
            
            _crouchStartTime = Time.time;
        }

        ApplyAnimation(crouchTarget.position);
    }

    private void Stand()
    {
        if (_wasCrouching)
        {
            onCrouchEnd.Invoke();
            crouchCollider.Disable();
            standingCollider.SafeEnable();
            
            _crouchStartTime = Time.time;
        }
                
        ApplyAnimation(standingTarget.position);
    }

    // Animate our transform's position with the SmoothStep function.
    // See: https://en.wikipedia.org/wiki/Smoothstep
    
    private void ApplyAnimation(Vector3 targetPosition)
    {
        float elapsedTime = Time.time - _crouchStartTime;
        float percentDone = math.smoothstep(0, crouchDuration, elapsedTime * Time.timeScale);
        
        crouchTransform.position = Vector3.Lerp(crouchTransform.position, targetPosition, percentDone);
    }
}