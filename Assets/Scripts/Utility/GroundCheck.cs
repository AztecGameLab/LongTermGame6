using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// - needs to provide more collision information for fall damage to be implemented
// - overall hard to understand, clean code

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Settings")]
    
    [SerializeField]
    [Tooltip("The downwards direction used to check if we are grounded.")]
    private Vector3 gravityDirection = Vector3.down;
    
    [SerializeField]
    [Tooltip("How steep a slope we can climb without slipping.")]
    private float slopeLimitDegrees = 45f;
    
    [SerializeField]
    [Tooltip("Draws debug information to the screen.")]
    private bool showDebug;

    [Space(20f)] 
    
    [SerializeField]
    [Tooltip("Passes the collider which was collided with, and the velocity on collision.")]
    private UnityEvent<Collider, Vector3> onEnterCollision;

    [SerializeField]
    [Tooltip("Passes the collider which was exited.")]
    private UnityEvent<Collider> onExitCollision;

    private Dictionary<Collider, Vector3> _collidersToNormals = new Dictionary<Collider, Vector3>();
    
    public bool WasGroundedLastFrame { get; private set; }
    public float TimeSpentGrounded { get; private set; }
    public float TimeSpentFalling { get; private set; }
    
    public bool JustEntered => IsGrounded && !WasGroundedLastFrame;
    public bool JustExited => !IsGrounded && WasGroundedLastFrame;
    
    public bool IsGrounded { get; private set; }
    public Vector3 ContactNormal { get; private set; }
    public Collider ConnectedCollider { get; private set; }

    private Vector3 _previousPosition;
    private Vector3 _currentPosition;
    private Vector3 _velocity;
    
    private void FixedUpdate()
    {
        WasGroundedLastFrame = IsGrounded;
        
        BetterGroundedUpdate();
        
        _previousPosition = _currentPosition;
        _currentPosition = transform.position;
        _velocity = (_currentPosition - _previousPosition) / Time.fixedDeltaTime;
        
        if (JustEntered)
            HandleCollisionEnter();
        
        if (JustExited)
            HandleCollisionExit();
    }

    private bool IsValidCollision(KeyValuePair<Collider, Vector3> pair)
    {
        Vector3 upDirection = -gravityDirection;
        Vector3 normalDirection = pair.Value;
        float slopeAngle = Vector3.Angle(upDirection, normalDirection);

        return slopeAngle <= slopeLimitDegrees;
    }

    private void BetterGroundedUpdate()
    {
        Vector3 sum = Vector3.zero;
        IsGrounded = false;
        ConnectedCollider = null;
        
        foreach (KeyValuePair<Collider,Vector3> pair in _collidersToNormals)
        {
            sum += pair.Value;
            
            if (IsValidCollision(pair))
            {
                IsGrounded = true;
                ConnectedCollider = pair.Key;
            }
        }

        ContactNormal = _collidersToNormals.Count > 0 ? sum / _collidersToNormals.Count : Vector3.zero;
    }

    private void HandleCollisionEnter()
    {
        TimeSpentFalling = 0;
        onEnterCollision.Invoke(ConnectedCollider, _velocity);
    }

    private void HandleCollisionExit()
    {
        TimeSpentGrounded = 0;
        onExitCollision.Invoke(ConnectedCollider);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.contactCount > 0)
            UpdateCollisionDictionary(other.collider, other.GetContact(0).normal);
    }
    
    private void UpdateCollisionDictionary(Collider targetCollider, Vector3 normal)
    {
        if (_collidersToNormals.ContainsKey(targetCollider) == false)
            _collidersToNormals.Add(targetCollider, normal);

        else _collidersToNormals[targetCollider] = normal;
    }

    private void OnCollisionExit(Collision other)
    {
        _collidersToNormals.Remove(other.collider);
    }
    
    private void Update()
    {
        if (IsGrounded)
            TimeSpentGrounded += Time.deltaTime;
        
        else TimeSpentFalling += Time.deltaTime;
    }

    #region Debug

        private void OnGUI()
        {
            if (showDebug)
            {
                string connectedCollider = ConnectedCollider ? ConnectedCollider.name : "None";
                    
                GUILayout.Label($"IsGrounded: {IsGrounded}");
                GUILayout.Label($"Was Grounded Last Frame: {WasGroundedLastFrame}");
                GUILayout.Label($"Connected Collider: {connectedCollider}");
                GUILayout.Label($"Contact Normal: {ContactNormal}");
                GUILayout.Label($"Time spent grounded: {TimeSpentGrounded}");
                GUILayout.Label($"Time spent falling: {TimeSpentFalling}");
                GUILayout.Label($"Velocity: {_velocity}");
            }
        }

    #endregion
}