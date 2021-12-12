using UnityEngine;

// todo: better system for checking disconnects
// todo: cleanup

[RequireComponent(typeof(Rigidbody))]
public class MovableObject : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float movementSpeed = 1f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private bool freezeRotation = true;
    
    private Transform _current;
    private bool _isHeld;
    private Rigidbody _rigidbody;
    private Transform _targetTransform;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _current = new GameObject("Current Position").transform;
        _current.parent = transform;
    }

    private void FixedUpdate()
    {
        if (_isHeld)
            MoveTowardsTarget();                   
    }

    private void MoveTowardsTarget()
    {
        Vector3 target = _targetTransform.position;
        Vector3 current = _current.position;
        Vector3 directionToTarget = target - current;
        

        _rigidbody.velocity = directionToTarget * movementSpeed / Time.fixedDeltaTime;
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxSpeed);
    }

    public void Grab(GameObject grabber, Vector3 point)
    {
        MovingTarget target = grabber.GetComponentInChildren<MovingTarget>();

        var transform1 = target.transform;
        _targetTransform = target == null ? grabber.transform : transform1;
        _isHeld = true;
        
        if (freezeRotation)
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            
        transform1.position = point;
        _current.position = point;
    }

    public void Drop()
    {
        _isHeld = false;
        
        if (freezeRotation)
           _rigidbody.constraints = RigidbodyConstraints.None;
    }

    private void OnCollisionEnter(Collision other)
    {
        //todo: make this less hardcoded
        if (other.gameObject.layer == 6)
            Drop();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == 6)
            Drop();
    }
}