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
    
    private bool _isCrouching;
    private bool _wasCrouching;
    private float _startTime;

    // Methods
    
    private void Update()
    {
        _wasCrouching = _isCrouching;

        if (ShouldCrouch())
            Crouch();
        
        else if (ShouldStand())
            Stand();
    }

    private bool ShouldCrouch()
    {
        bool isGrounded = groundCheck.IsGrounded;
        return WantsToCrouch && isGrounded && crouchCollider.IsClear;
    }
    
    private bool ShouldStand()
    {
        bool isGrounded = groundCheck.IsGrounded;
        return !WantsToCrouch && isGrounded && standingCollider.IsClear;
    }

    private void Crouch()
    {
        if (!_wasCrouching)
        {
            _startTime = Time.time;
            _isCrouching = true;
            onCrouchStart.Invoke();
            crouchCollider.SafeEnable();
            standingCollider.Disable();
        }

        // Animate our transform's position with the SmoothStep function.
        // See: https://en.wikipedia.org/wiki/Smoothstep
                
        float percentDone = math.smoothstep(0, crouchDuration, (Time.time - _startTime) * Time.timeScale);
        crouchTransform.position = Vector3.Lerp(crouchTransform.position, crouchTarget.position, percentDone);
    }

    private void Stand()
    {
        if (_wasCrouching)
        {
            _startTime = Time.time;
            _isCrouching = false;
            onCrouchEnd.Invoke();
            crouchCollider.Disable();
            standingCollider.SafeEnable();
        }
        
        // Animate our transform's position with the SmoothStep function.
        // See: https://en.wikipedia.org/wiki/Smoothstep
                
        float percentDone = math.smoothstep(0, crouchDuration, (Time.time - _startTime) * Time.timeScale);
        crouchTransform.position = Vector3.Lerp(crouchTransform.position, standingTarget.position, percentDone);
    }
}