using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
// Events that provide information about a collision.
public struct CollisionEvents 
{
    public UnityEvent<Collider> collisionEnter;
    public UnityEvent<Collider> collisionExit;
}
    
// Wraps a trigger Collider and provides UnityEvents for OnTriggerEnter / Exit / Stay
[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
    public CollisionEvents events;
    
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        events.collisionEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        events.collisionExit.Invoke(other);
    }
}