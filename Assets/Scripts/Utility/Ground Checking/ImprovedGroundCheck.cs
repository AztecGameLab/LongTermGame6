﻿using UnityEngine;
using UnityEngine.Events;

// todo: finish testing + try to integrate with player as an upgrade.

// important aspects: 
// - get speed before collision (for fall damage, ect.)
// - quickly and accurately update grounded info (no frame-late events, everything is on time)
// - easy to read, clean

[RequireComponent(typeof(Collider))]
public class ImprovedGroundCheck : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float groundedAngle = 45f;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onTouchGround;
    [SerializeField] private UnityEvent onLeaveGround;

    public bool IsGrounded { get; private set; }

    private void OnCollisionEnter(Collision other)
    {
        if (other.contactCount > 0 && Vector3.Angle(other.contacts[0].normal, Vector3.up) <= groundedAngle)
        {
            Debug.Log("Touched Ground");
            IsGrounded = true;
            onTouchGround.Invoke();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.relativeVelocity != Vector3.zero && Vector3.Angle(-other.relativeVelocity, Vector3.up)<= groundedAngle)
        {
            Debug.Log("Left Ground");
            IsGrounded = false;
            onLeaveGround.Invoke();
        }
    }
}