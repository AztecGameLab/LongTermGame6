using JetBrains.Annotations;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private Vector3 gravityDirection = Vector3.down;
    
    [PublicAPI] public float gravity;
    [PublicAPI] public Vector3 GravityDirection => gravityDirection;

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        targetRigidbody.AddForce(GravityDirection * gravity, ForceMode.Acceleration);
    }
}