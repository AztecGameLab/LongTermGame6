using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private UnityEvent<GameObject, Vector3> onInteractStart;
    [SerializeField] private UnityEvent onInteractEnd;

    public float InteractRange => interactRange;
    
    public void InteractStart(GameObject source, Vector3 point)
    {
        onInteractStart.Invoke(source, point);
    }

    public void InteractEnd()
    {
        onInteractEnd.Invoke();
    }
}