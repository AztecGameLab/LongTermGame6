using UnityEngine;

// todo: make this actually work

[RequireComponent(typeof(Rigidbody))]
public class MovableObject : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform center;
    
    private bool _isHeld;
    private Rigidbody _rigidbody;
    private Transform _targetTransform;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isHeld)
            MoveTowardsTarget();                   
    }

    private void MoveTowardsTarget()
    {
        Vector3 target = _targetTransform.position;
        Vector3 current = center.position;
        Vector3 directionToTarget = target - current;
        
        Debug.DrawLine(current, target, Color.red);
        
        float distanceToTarget = Vector3.Distance(target, current);

        _rigidbody.velocity = directionToTarget;
    }

    public void Grab(GameObject grabber)
    {
        MovingTarget target = grabber.GetComponentInChildren<MovingTarget>();

        
        _targetTransform = target == null ? grabber.transform : target.transform;
        _isHeld = true;
    }

    public void Drop()
    {
        _isHeld = false;
    }
}