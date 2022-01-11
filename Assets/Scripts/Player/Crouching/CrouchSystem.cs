using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

public class CrouchSystem : MyNamespace.System
{
    // [Header("Settings")]
    // [SerializeField] private float radiusBuffer = 0.1f;
    // [SerializeField] private float heightBuffer = 0.2f;

    [Header("Dependencies")] 
    [SerializeField] private Trigger crouchTrigger;
    [SerializeField] private Collider crouchCollider;
    
    [SerializeField] private Trigger standingTrigger;
    [SerializeField] private Collider standingCollider;

    [Space(20)]
    
    [SerializeField] private UnityEvent onCrouchStart;
    [SerializeField] private UnityEvent onCrouchEnd;
    
    public bool WantsToCrouch { get; set; }
    public bool IsCrouching { get; private set; }
    
    private bool _wasCrouching;

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
        return standingTrigger.IsOccupied;
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
        crouchCollider.enabled = IsCrouching;
        standingCollider.enabled = !IsCrouching;
    }
}