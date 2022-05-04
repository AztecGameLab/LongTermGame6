using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents an object that can be interacted with (pressing a button, grabbing a box, turning a wheel ect).
/// </summary>

public class Interactable : MonoBehaviour
{
    [Header("Settings")]
    
    [SerializeField] 
    [Tooltip("How close must you be to this object to interact with it.")]
    private float interactRange = 1.5f;
    
    [SerializeField] 
    [Tooltip("How often will this object check if its line of sight is interupted from the player. If it is it will drop itself")]
    private float lineOfSightCheckInterval = 1.5f;
    [Space(20f)]
    
    [SerializeField]
    [Tooltip("Passes the object interacting with us, and the position they selected.")]
    public UnityEvent<GameObject, Vector3> onInteractStart;
    
    [SerializeField]
    [Tooltip("Called when this object stops being interacted with.")]
    public UnityEvent onInteractEnd;
    // Internal State
    
    public float InteractRange => interactRange;
    public bool IsHeld { get; private set; }
    public Vector3 GrabPoint { get; private set; }
    
    private int _ignoreRaycastLayer;
    private Transform _cameraTransform;
    private float _lastLineOfSightCheck;
    
    // Methods

    private void Start()
    {
        _ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
        
        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;
    }

    public void InteractStart(GameObject source, Vector3 point)
    {
        if (!IsHeld)
        {
            onInteractStart.Invoke(source, point);
            GrabPoint = point;
            IsHeld = true;
        
            _lastLineOfSightCheck = Time.time;
        }
    }

    public void InteractEnd()
    {
        if (IsHeld)
        {
            IsHeld = false;
            onInteractEnd.Invoke();
        }
    }

    private void Update()
    {
        if(IsHeld && lineOfSightCheckInterval < Time.time - _lastLineOfSightCheck)
        {
            _lastLineOfSightCheck = Time.time;
            CheckForObstruction();
        }
    }

    private void CheckForObstruction()
    {
        Vector3 directionToPlayer = _cameraTransform.position - transform.position;
        GameObject self = gameObject;
            
        // Move this object to the "Ignore Raycast" layer so we don't count ourself as a blocking object. 
        int originalLayer = self.layer;
        self.layer = _ignoreRaycastLayer;
            
        if (Physics.Raycast(transform.position, directionToPlayer, out var hit))
        {
            // Drop this object if something is obstructing our line to the player.
            if (hit.transform.CompareTag("Player") == false)
                InteractEnd();
        }
            
        gameObject.layer = originalLayer;
    }
}