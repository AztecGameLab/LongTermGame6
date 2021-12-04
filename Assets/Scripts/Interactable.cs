using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private UnityEvent onInteractStart;
    [SerializeField] private UnityEvent onInteractEnd;

    public float InteractRange => interactRange;
    
    public void InteractStart()
    {
        onInteractStart.Invoke();
    }

    public void InteractEnd()
    {
        onInteractEnd.Invoke();
    }
}