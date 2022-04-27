using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Tracks and emits an event that represents a step being taken in the game.
/// <remarks>Mainly designed for playing footstep audio, but is still pretty general.</remarks>
/// </summary>

// todo: send more good information to FMOD for playing audio - crouch state, current surface, walk, jump, fall

public class StepTracker : MonoBehaviour
{
    [Header("Settings")]
    
    [SerializeField]
    [Range(0, 5)]
    [Tooltip("How far we can walk before registering a step.")]
    private float stepDistance = 2f;

    [SerializeField]
    [Tooltip("The sound profile of the entity making the steps")]
    private EntitySoundProfile soundProfile;
    [Header("Dependencies")]
    
    [SerializeField]
    [Tooltip("We should only register steps when on the ground.")]
    private GroundCheck groundCheck;
    
    [SerializeField]
    [Tooltip("Used to determine how far this object has moved every frame.")]
    private Transform movementSource;

    [Space(20f)]
    
    [Tooltip("Called every time we take a step (as defined above).")]
    [SerializeField] 
    private UnityEvent stepEvent;

    // Internal State
    
    private Vector3 _previousPosition;
    private float _elapsedDistance;
    
    // Methods
    
    private void Update()
    {
        if (groundCheck == null || groundCheck.IsGrounded)
            TrackMovement();
    }

    private void TrackMovement()
    {
        _elapsedDistance += (movementSource.position - _previousPosition).magnitude;

        if (_elapsedDistance > stepDistance)
        {
            string footStepSound = soundProfile.GetSoundOnSurface(groundCheck.ConnectedCollider.tag);
            if(footStepSound!="")
                RuntimeManager.PlayOneShot(footStepSound,transform.position);
            stepEvent.Invoke();
            _elapsedDistance = 0f;
        }
    }

    private void LateUpdate()
    {
        _previousPosition = movementSource.position;
    }
}