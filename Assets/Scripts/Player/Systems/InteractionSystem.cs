using JetBrains.Annotations;
using UnityEngine;

// todo: clean up code here...I want a way to alert interactable when its hovered + get currently hovered interactable

public class InteractionSystem : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private bool showDebug;
    
    [Header("Dependencies")]
    [SerializeField] private Transform lookDirection;

    private Interactable _targetInteractable;
    private bool _hasTargetInteractable;
    private bool _wasHoldingInteract;
    private RaycastHit[] _hits = new RaycastHit[100];

    [PublicAPI] public bool HoldingInteract { get; set; }
    
    private void Update()
    {
        int hits = Physics.RaycastNonAlloc(lookDirection.position, lookDirection.forward, _hits);

        for (int i = 0; i < hits; i++)
        {
            RaycastHit currentHit = _hits[i];
            
            if (currentHit.transform.TryGetComponent(out _targetInteractable) && currentHit.distance <= _targetInteractable.InteractRange)
            {
                _hasTargetInteractable = true;
                break;
            }

            _hasTargetInteractable = false;
        }

        if (_hasTargetInteractable && !_wasHoldingInteract && HoldingInteract)
            _targetInteractable.Interact();

        _wasHoldingInteract = HoldingInteract;
    }

    private void OnGUI()
    {
        if (showDebug)
            DrawDebugUI();
    }

    private void DrawDebugUI()
    {
        GUILayout.Label($"Holding Interact: {HoldingInteract}");
        GUILayout.Label($"Target interactable: {(_hasTargetInteractable ? _targetInteractable.name : "None")}");
        GUILayout.Label($"Look direction: {lookDirection}");
    }
}