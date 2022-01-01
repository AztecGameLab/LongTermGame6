using UnityEngine;
using UnityEngine.Events;

public class CrouchSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float radiusBuffer = 0.1f;
    [SerializeField] private float heightBuffer = 0.2f;

    [Header("Dependencies")] 
    [SerializeField] private CapsuleCollider standingCollider;
    [SerializeField] private CapsuleCollider crouchingCollider;

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
        Ray upwardsRay = new Ray(standingCollider.bounds.center, standingCollider.transform.up);
        
        // We divide the height by 2 because our ray is starting at the center of the object, not the bottom.
        
        float radiusOfObject = standingCollider.radius - radiusBuffer;
        float heightOfObject = standingCollider.height / 2 - heightBuffer;

        return Physics.SphereCast(upwardsRay, radiusOfObject, heightOfObject);
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
        standingCollider.isTrigger = IsCrouching;
        crouchingCollider.isTrigger = !IsCrouching;
    }
}