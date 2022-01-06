using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float interactRange = 1.5f;
    
    [Space(20f)]
    [SerializeField] public UnityEvent<GameObject, Vector3> onInteractStart;
    [SerializeField] public UnityEvent onInteractEnd;

    public float InteractRange => interactRange;
    public bool IsHeld { get; private set; }
    public Vector3 GrabPoint { get; private set; }
    
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