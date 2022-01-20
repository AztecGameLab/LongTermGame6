using JetBrains.Annotations;
using UnityEngine;

// todo: big cleanup

public class InteractionSystem : MyNamespace.System
{
    [Header("Settings")] 
    [SerializeField] private bool showDebug;
    [SerializeField] private int playerLayer = 6;
    
    [Header("Dependencies")]
    [SerializeField] public Transform lookDirection;

    private bool _wasHoldingInteract;
    private bool JustReleasedInteract => _wasHoldingInteract && !HoldingInteract;
    private bool JustPressedInteract => !_wasHoldingInteract && HoldingInteract;
    
    [PublicAPI] public bool LookingAtInteractable { get; private set; }
    [PublicAPI] public bool HoldingInteract { get; set; }
    [PublicAPI] public Interactable CurrentInteractable { get; private set; }
    [PublicAPI] public bool HasInteractable => CurrentInteractable != null;
    
    private void Update()
    {
        if (TryGetInteractable(out Interactable visionInteractable, out Vector3 point))
        {
            LookingAtInteractable = true;
            
            if (JustPressedInteract)
                GrabObject(visionInteractable, point);
        }

        else LookingAtInteractable = false;

        if (JustReleasedInteract)
            DropCurrentObject();
        
        _wasHoldingInteract = HoldingInteract;
    }

    private void GrabObject(Interactable interactable, Vector3 point)
    {
        DropCurrentObject();

        CurrentInteractable = interactable;
        CurrentInteractable.InteractStart(gameObject, point);
    }

    private void DropCurrentObject()
    {
        if (CurrentInteractable != null)
            CurrentInteractable.InteractEnd();

        CurrentInteractable = null;
    }

    private bool TryGetInteractable(out Interactable result, out Vector3 point)
    {
        int layerMask = ~(1 << playerLayer);
        Ray visionRay = new Ray(lookDirection.position, lookDirection.forward);
        bool lookingAtObject = Physics.Raycast(visionRay, out RaycastHit hitInfo, float.PositiveInfinity, layerMask);

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