﻿using JetBrains.Annotations;
using UnityEngine;

// todo: support real-time changing of constraints, e.g. for ladders or other gameplay things
// todo v2: turn settings into ScriptableObject and support blending between them?

/// <summary>
/// Allows for constrained updating of the pitch, yaw, and roll of an object.
/// </summary>

public class RotationSystem : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float pitchConstraint;
    [SerializeField] private float yawConstraint;
    [SerializeField] private float rollConstraint;
    [SerializeField] private bool showDebug;

    [Header("Dependencies")]
    [SerializeField] private Transform pitchTransform;
    [SerializeField] private Transform yawTransform;
    [SerializeField] private Transform rollTransform;

    private Vector3 _initialRotation;
    private Vector3 _currentRotation;
    
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

    private void Awake()
    {
        // Find and save our current euler rotation as a starting point.
        
        if (pitchTransform != null) _currentRotation.x = pitchTransform.localRotation.eulerAngles.x;
        if (yawTransform != null)   _currentRotation.y = yawTransform.localRotation.eulerAngles.y;
        if (rollTransform != null)  _currentRotation.z = rollTransform.localRotation.eulerAngles.z;
        
        _initialRotation = _currentRotation;
    }

    private static float Constrain(float value, float initialValue, float constraint)
    {
        if (constraint < 0)
            return value;
        
        // Ensure that the provided value cannot deviate too far from the initial value, specified by the constraint. 
        
        return Mathf.Clamp(value, initialValue - constraint, initialValue + constraint);
    }

    #region Debug

    private void OnGUI()
    {
        if (showDebug) 
            GUILayout.Label($"Current Rotation: {CurrentRotation}");
    }

    #endregion
}