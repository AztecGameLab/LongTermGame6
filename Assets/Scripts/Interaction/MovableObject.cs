﻿using JetBrains.Annotations;
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
    
    // The current position of this object
    private Transform _currentPosition;

    //The rotation of object when picked up
    private Quaternion offsetRot;

    private Transform _cameraTransform;

    // The position this object is trying to accelerate towards.
    public Transform TargetTransform { get; private set; }

    // Is this object currently colliding with something?
    public bool HasCollision { get; private set; }

    [PublicAPI] public Interactable Interactable { get; private set; }
    [PublicAPI] public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Interactable = GetComponent<Interactable>();
        
        _currentPosition = new GameObject("Current Position").transform;
        _currentPosition.parent = transform;

        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;
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

        TargetTransform = target == null ? grabber.transform : target.transform;
        TargetTransform.position = point;
        
        _currentPosition.position = point;

        offsetRot = Quaternion.Inverse(_cameraTransform.rotation) * transform.rotation;
    }


    private void Update()
    {
        if (Interactable.IsHeld)
        {
            RotateTowardsTarget();
        }

    }
    private void FixedUpdate()
    {
        if (Interactable.IsHeld)
        {
            MoveTowardsTarget();
        }
                             
    }

    private void MoveTowardsTarget()
    {
        Vector3 target = TargetTransform.position;
        Vector3 current = _currentPosition.position;
        Vector3 directionToTarget = target - current;

        Rigidbody.velocity = directionToTarget * movementSpeed / Time.fixedDeltaTime;
        Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, maxSpeed);
    }

    private void RotateTowardsTarget()
    {
        if (Interactable.GetComponent<HingeJoint>() == null)//make sure its not a door
        {// thinking of making doors tagged or tag all objects that dont require the rotation

            Rigidbody.MoveRotation(_cameraTransform.rotation * offsetRot);

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        HasCollision = true;
        
        if (other.gameObject.layer == playerLayer)
            Interactable.InteractEnd();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == playerLayer)
            Interactable.InteractEnd();
    }
    private void OnCollisionExit()
    {
        HasCollision = false;
    }
}