using UnityEngine;
using UnityEngine.Events;

public class StepTracker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Range(0, 5)] private float stepDistance = 2f;

    [Header("Dependencies")]
    [SerializeField] private GroundedCheck groundCheck;
    [SerializeField] private Transform movementSource;

    [Header("Events")]
    [SerializeField] private UnityEvent stepEvent;

    private Vector3 _previousPosition;
    private float _elapsedDistance;
    
    private void Update()
    {
        if (groundCheck.IsGrounded)
            TrackMovement();
    }

    private void TrackMovement()
    {
        _elapsedDistance += (movementSource.position - _previousPosition).magnitude;

        if (_elapsedDistance > stepDistance)
        {
            stepEvent.Invoke();
            _elapsedDistance = 0f;
        }
    }

    private void LateUpdate()
    {
        _previousPosition = movementSource.position;
    }
}