using JetBrains.Annotations;
using UnityEngine;

// todo: big cleanup

public class InteractionSystem : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private bool showDebug;
    
    [Header("Dependencies")]
    [SerializeField] private Transform lookDirection;

    public bool LookingAtInteractable { get; private set; }
    private bool _test;
    private bool _wasHoldingInteract;
    private Interactable _targetInteractable;
    private RaycastHit[] _hits = new RaycastHit[100];

    private bool JustReleasedInteract => _wasHoldingInteract && !HoldingInteract;
    private bool JustPressedInteract => !_wasHoldingInteract && HoldingInteract;
    
    [PublicAPI] public bool HoldingInteract { get; set; }
    
    private void Update()
    {
        if (TryGetInteractable(out Interactable target))
        {
            LookingAtInteractable = true;
            
            if (JustPressedInteract)
            {
                target.InteractStart(gameObject);
                _test = true;
            }

            if (JustReleasedInteract && _test)
            {
                target.InteractEnd(gameObject);
                _test = false;
            }

            _targetInteractable = target;
        }
        
        else if (_targetInteractable != null && _test)
        {
            _targetInteractable.InteractEnd(gameObject);
            _targetInteractable = null;
            _test = false;
            LookingAtInteractable = false;
        }

        else LookingAtInteractable = false;

        _wasHoldingInteract = HoldingInteract;
    }

    private bool TryGetInteractable(out Interactable result)
    {
        result = null;
        
        return Physics.Raycast(new Ray(lookDirection.position, lookDirection.forward), out RaycastHit hitInfo) &&
            hitInfo.transform.TryGetComponent(out result) && hitInfo.distance <= result.InteractRange;
    }

    private void OnGUI()
    {
        if (showDebug)
            DrawDebugUI();
    }

    private void DrawDebugUI()
    {
        GUILayout.Label($"Holding Interact: {HoldingInteract}");
        GUILayout.Label($"Target interactable: {(TryGetInteractable(out Interactable result) ? result.name : "None")}");
        GUILayout.Label($"Look direction: {lookDirection}");
    }
}