using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private UnityEvent<GameObject> onInteractStart;
    [SerializeField] private UnityEvent<GameObject> onInteractEnd;

    public float InteractRange => interactRange;
    
    public void InteractStart(GameObject source)
    {
        onInteractStart.Invoke(source);
    }

    public void InteractEnd(GameObject source)
    {
        onInteractEnd.Invoke(source);
    }
}