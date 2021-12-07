using Cinemachine.Utility;
using UnityEngine;

// todo: make this actually work

[RequireComponent(typeof(Rigidbody))]
public class MovableObject : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    
    private bool _isHeld;
    private Rigidbody _rigidbody;
    private Transform _targetTransform;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_isHeld)
            MoveTowardsTarget();                   
    }

    private void MoveTowardsTarget()
    {
        Vector3 target = _targetTransform.position;
        Vector3 current = targetRenderer.bounds.center;
        Vector3 directionToTarget = target - current;
        
        // float distanceToTarget = Vector3.Distance(target, current);

        _rigidbody.velocity = directionToTarget  / Time.fixedDeltaTime;
    }

    public void Grab(GameObject grabber)
    {
        MovingTarget target = grabber.GetComponentInChildren<MovingTarget>();


        var transform1 = target.transform;
        _targetTransform = target == null ? grabber.transform : transform1;
        _isHeld = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        transform1.position = targetRenderer.bounds.center;
    }

    public void Drop()
    {
        _isHeld = false;
        _rigidbody.constraints = RigidbodyConstraints.None;
    }
}