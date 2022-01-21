using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Allows an object to interact with other objects in the world.
/// </summary>

// todo: big cleanup

public class InteractionSystem : MyNamespace.System
{
    [Header("Settings")] 
    
    [SerializeField]
    [Tooltip("Writes debug information to the screen.")]
    private bool showDebug;
    
    [SerializeField] 
    [Tooltip("Which layers should be ignored when looking for interactables.")]
    private LayerMask ignoredLayers;
    
    [Header("Dependencies")]
    
    [SerializeField] 
    [Tooltip("The transform used to calculate what direction we are looking.")]
    public Transform lookDirection;

    // Internal State
    
    private bool _wasHoldingInteract;
    private bool JustReleasedInteract => _wasHoldingInteract && !HoldingInteract;
    private bool JustPressedInteract => !_wasHoldingInteract && HoldingInteract;
    
    [PublicAPI] public bool LookingAtInteractable { get; private set; }
    [PublicAPI] public bool HoldingInteract { get; set; }
    [PublicAPI] public Interactable CurrentInteractable { get; private set; }
    [PublicAPI] public bool HasInteractable => CurrentInteractable != null;
    
    // Methods
    
    private void Update()
    {
        if (TryGetInteractable(out Interactable visionInteractable, out Vector3 point))
        {
            LookingAtInteractable = true;
            
            if (JustPressedInteract)
                GrabObject(visionInteractable, point);
        }

        else LookingAtInteractable = false;

        if (JustReleasedInteract && HasInteractable)
            CurrentInteractable.InteractEnd();
        
        _wasHoldingInteract = HoldingInteract;
    }

    private void GrabObject(Interactable interactable, Vector3 point)
    {
        if (HasInteractable)
            CurrentInteractable.InteractEnd();
        
        CurrentInteractable = interactable;
        CurrentInteractable.InteractStart(gameObject, point);
        
        CurrentInteractable.onInteractEnd.AddListener(DropCurrentObject);
    }

    private void DropCurrentObject()
    {
        CurrentInteractable.onInteractEnd.RemoveListener(DropCurrentObject);
        CurrentInteractable = null;
    }

    private bool TryGetInteractable(out Interactable result, out Vector3 point)
    {
        Ray visionRay = new Ray(lookDirection.position, lookDirection.forward);
        bool lookingAtObject = Physics.Raycast(visionRay, out RaycastHit hitInfo, float.PositiveInfinity, ~ignoredLayers.value, QueryTriggerInteraction.Ignore);

        if (lookingAtObject && hitInfo.rigidbody != null)
        {
            bool isInteractable = hitInfo.rigidbody.TryGetComponent(out result);
            bool isInRange = isInteractable && hitInfo.distance <= result.InteractRange;
            point = hitInfo.point;
            
            return isInteractable && isInRange;
        }

        point = Vector3.zero;
        result = null;
        return false;
    }

    private void OnGUI()
    {
        if (showDebug)
            DrawDebugUI();
    }

    private void DrawDebugUI()
    {
        GUILayout.Label($"Holding Interact: {HoldingInteract}");
        GUILayout.Label($"Target interactable: {(TryGetInteractable(out Interactable result, out Vector3 _) ? result.name : "None")}");
        GUILayout.Label($"Look direction: {lookDirection}");
    }
}