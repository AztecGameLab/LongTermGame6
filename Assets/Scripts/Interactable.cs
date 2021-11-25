using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private UnityEvent onInteract = new UnityEvent();

    public float InteractRange => interactRange;
    
    public void Interact()
    {
        Debug.Log("interacted with " + gameObject.name);
        onInteract.Invoke();
    }
}