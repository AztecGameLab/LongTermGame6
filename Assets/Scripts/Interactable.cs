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
        Debug.Log("Started interaction with " + gameObject.name);
        onInteractStart.Invoke();
    }

    public void InteractEnd()
    {
        Debug.Log("Ended interaction with " + gameObject.name);
        onInteractEnd.Invoke();
    }
}