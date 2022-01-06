using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

// todo: better system for checking disconnects

[RequireComponent(typeof(Rigidbody), typeof(Interactable))]
public class MovableObject : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float movementSpeed = 0.5f;
    [SerializeField, Layer] private int playerLayer = 6;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private bool freezeRotation = true;
    
    private Transform _current;
    private Transform _targetTransform;

    [PublicAPI] public Interactable Interactable { get; private set; }
    [PublicAPI] public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Interactable = GetComponent<Interactable>();
        
        _current = new GameObject("Current Position").transform;
        _current.parent = transform;
    }
    
    private void OnEnable()
    {
        Interactable.onInteractStart.AddListener(HandleInteractStart);
        Interactable.onInteractEnd.AddListener(HandleInteractEnd);
    }

    private void OnDisable()
    {
        Interactable.onInteractStart.RemoveListener(HandleInteractStart);
        Interactable.onInteractEnd.RemoveListener(HandleInteractEnd);
    }

    private void HandleInteractEnd()
    {
        if (freezeRotation)
            Rigidbody.constraints = RigidbodyConstraints.None;
    }

    private void HandleInteractStart(GameObject grabber, Vector3 point)
    {
        if (freezeRotation)
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        
        MovingTarget target = grabber.GetComponentInChildren<MovingTarget>();

        _targetTransform = target == null ? grabber.transform : target.transform;
        _targetTransform.position = point;
        _current.position = point;
    }

    private void FixedUpdate()
    {
        if (Interactable.IsHeld)
            MoveTowardsTarget();                   
    }

    private void MoveTowardsTarget()
    {
        Vector3 target = _targetTransform.position;
        Vector3 current = _current.position;
        Vector3 directionToTarget = target - current;
        
        Rigidbody.velocity = directionToTarget * movementSpeed / Time.fixedDeltaTime;
        Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, maxSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == playerLayer)
            Interactable.InteractEnd();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == playerLayer)
            Interactable.InteractEnd();
    }
}