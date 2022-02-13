using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public Vector3 gravityDirection = Vector3.down;
    [SerializeField] public float gravityMagnitude = 9.8f;
    
    private Rigidbody _rigidbody;

    public Vector3 GravityDirection => gravityDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 gravityForce = GravityDirection * gravityMagnitude;
        _rigidbody.AddForce(gravityForce, ForceMode.Acceleration);
    }
}