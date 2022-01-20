using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

// Wraps a trigger Collider and provides UnityEvents for OnTriggerEnter / Exit / Stay
[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
    // todo: use actual LayerMask because its more powerful, but I don't wanna think about the bitwise ops right now
    [SerializeField, Layer] private int excludeLayer;
    [SerializeField] private bool showDebug;

    [Space(20f)]
    [SerializeField] private UnityEvent<Collider> collisionEnter;
    [SerializeField] private UnityEvent<Collider> collisionExit;

    public bool IsOccupied { get; private set; }
    private bool _hasObjectInTrigger;
    
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void FixedUpdate()
    {
        IsOccupied = _hasObjectInTrigger;
        _hasObjectInTrigger = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsValidCollider(other))
            _hasObjectInTrigger = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsValidCollider(other))
            collisionEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsValidCollider(other))
            collisionExit.Invoke(other);
    }
    
    private bool IsValidCollider(Collider other)
    {
        return other.gameObject.layer != excludeLayer;
    }

    private void OnGUI()
    {
        if (showDebug)
            GUILayout.Label($"Is occupied: {IsOccupied}");
    }
}