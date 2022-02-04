using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Allows an object to switch between colliders in a safe way, without clipping into walls.
/// </summary>

public class CrouchSystem : MyNamespace.System
{
    [Header("Dependencies")] 
    
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
    public bool IsCrouching { get; private set; }
    
    private bool _wasCrouching;

    // Methods
    
    private void Update()
    {
        // Ensure that we are only able to change our crouching state if there is room to stand.
        
        if (CheckIfBlockedAbove() == false)
            IsCrouching = WantsToCrouch;
     
        UpdateEvents();
        UpdateCollider();
        
        _wasCrouching = IsCrouching;
    }
    
    private bool CheckIfBlockedAbove()
    {
        return standingTrigger.Colliders.Count > 0;
    }
    
    private void UpdateEvents()
    {
        bool crouchJustStarted = IsCrouching && !_wasCrouching;
        bool crouchJustEnded = !IsCrouching && _wasCrouching;
        
        if (crouchJustStarted)
            onCrouchStart.Invoke();
        
        else if (crouchJustEnded)
            onCrouchEnd.Invoke();
    }
    
    private void UpdateCollider()
    {
        crouchCollider.gameObject.SetActive(IsCrouching);
        standingCollider.gameObject.SetActive(!IsCrouching);
    }
}