using JetBrains.Annotations;
using UnityEngine;

// todo: support real-time changing of constraints, e.g. for ladders or other gameplay things
// todo v2: turn settings into ScriptableObject and support blending between them?

/// <summary>
/// Allows for constrained updating of the pitch, yaw, and roll of an object.
/// </summary>

public class RotationSystem : MyNamespace.System
{
    [Header("Settings")] 
    
    [SerializeField] 
    [Tooltip("The max and min value for pitch (X). [-1 means no constraint]")]
    private float pitchConstraint = 90f;
    
    [SerializeField]
    [Tooltip("The max and min value for yaw (Y). [-1 means no constraint]")]
    private float yawConstraint = -1f;
    
    [SerializeField]
    [Tooltip("The max and min value for roll (Z). [-1 means no constraint]")]
    private float rollConstraint = -1f;
    
    [Header("Dependencies")]
    
    [SerializeField] 
    [Tooltip("The transform to control X rotation.")]
    private Transform pitchTransform;
    
    [SerializeField]
    [Tooltip("The transform to control Y rotation.")]
    private Transform yawTransform;
    
    [SerializeField] 
    [Tooltip("The transform to control Z rotation.")]
    private Transform rollTransform;

    // Internal State

    private Vector3 _initialRotation;
    private Vector3 _currentRotation;

    public Transform PitchTransform => pitchTransform;
    public Transform YawTransform => yawTransform;
    public Transform RollTransform => rollTransform;

    // Methods
    
    private void Awake()
    {
        Activate();
    }
    
    public void Activate()
    {
        // Find and save our current euler rotation as a starting point.

        Transform targetTransform = transform;

        if (pitchTransform != null) _currentRotation.x = pitchTransform.localRotation.eulerAngles.x;
        if (yawTransform != null)   _currentRotation.y = yawTransform.localRotation.eulerAngles.y;
        if (rollTransform != null)  _currentRotation.z = rollTransform.localRotation.eulerAngles.z;

        CurrentRotation = targetTransform.rotation.eulerAngles;
        targetTransform.rotation = Quaternion.identity;

        _initialRotation = _currentRotation;
    }
    
    public void Deactivate()
    {
        transform.rotation = Quaternion.Euler(CurrentRotation);
        CurrentRotation = Vector3.zero;
    }
    
    [PublicAPI] public Vector3 Forward
    {
        get => Quaternion.Euler(_currentRotation) * Vector3.forward;

        set
        {
            Vector3 rotation = Quaternion.LookRotation(value).eulerAngles;
            
            _currentRotation.x = rotation.x % 360;
            _currentRotation.y = rotation.y % 360;
            _currentRotation.z = rotation.z % 360;
            
            pitchTransform.localRotation = Quaternion.Euler(_currentRotation.x, 0, 0);
            yawTransform.localRotation = Quaternion.Euler(0, _currentRotation.y, 0);
            rollTransform.localRotation = Quaternion.Euler(0, 0, _currentRotation.z);
        }
    }
    
    [PublicAPI] public Vector3 CurrentRotation
    {
        get => _currentRotation;
        set
        {
            Pitch = value.x;
            Yaw = value.y;
            Roll = value.z;
        }
    }

    [PublicAPI] public float Pitch
    {
        get => _currentRotation.x;
        set
        {
            float pitch = Constrain(value, _initialRotation.x, pitchConstraint);
            _currentRotation.x = pitch % 360;
            pitchTransform.localRotation = Quaternion.Euler(_currentRotation.x, 0, 0);    
        }
    }

    [PublicAPI] public float Yaw
    {
        get => _currentRotation.y;
        set
        {
            float yaw = Constrain(value, _initialRotation.y, yawConstraint);
            _currentRotation.y = yaw % 360;
            yawTransform.localRotation = Quaternion.Euler(0, _currentRotation.y, 0);
        }
    }

    [PublicAPI] public float Roll
    {
        get => _currentRotation.z;
        set
        {
            float roll = Constrain(value, _initialRotation.z, rollConstraint);
            _currentRotation.z = roll % 360;
            rollTransform.localRotation = Quaternion.Euler(0, 0, _currentRotation.z);
        }
    }

    private static float Constrain(float value, float initialValue, float constraint)
    {
        if (constraint < 0)
            return value;
        
        // Ensure that the provided value cannot deviate too far from the initial value, specified by the constraint. 
        
        return Mathf.Clamp(value, initialValue - constraint, initialValue + constraint);
    }
}