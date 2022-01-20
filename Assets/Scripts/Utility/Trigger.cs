using System.Collections.Generic;
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

    public IEnumerable<Rigidbody> Occupants => _occupants;
    public bool IsOccupied { get; private set; }

    private List<Rigidbody> _occupants;
    private bool _hasObjectInTrigger;
    
    private void Awake()
    {
        _occupants = new List<Rigidbody>();
        
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
        {
            if (!_occupants.Contains(other.attachedRigidbody))
                _occupants.Add(other.attachedRigidbody);
            
            _hasObjectInTrigger = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsValidCollider(other))
        {
            if (!_occupants.Contains(other.attachedRigidbody))
                _occupants.Add(other.attachedRigidbody);
            
            collisionEnter?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsValidCollider(other) || _occupants.Contains(other.attachedRigidbody))
        {
            _occupants.Remove(other.attachedRigidbody);
            collisionExit.Invoke(other);
        }
    }
    
    private bool IsValidCollider(Collider other)
    {
        bool hasRigidbody = other.attachedRigidbody != null;
        bool isTrigger = other.isTrigger;
        bool isExcludeLayer = other.gameObject.layer == excludeLayer;
        
        return hasRigidbody && !isTrigger && !isExcludeLayer;
    }

    private void OnGUI()
    {
        if (showDebug)
            GUILayout.Label($"Is occupied: {IsOccupied}");
    }
}