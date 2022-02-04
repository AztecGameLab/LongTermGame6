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
    
    // Methods
    
    public void InteractStart(GameObject source, Vector3 point)
    {
        GrabPoint = point;
        IsHeld = true;
        onInteractStart.Invoke(source, point);
    }

    public void InteractEnd()
    {
        IsHeld = false;
        onInteractEnd.Invoke();
    }
}