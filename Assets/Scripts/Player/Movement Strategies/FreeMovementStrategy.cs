using UnityEngine;

[CreateAssetMenu]
public class FreeMovementStrategy : MovementStrategy
{
    [Header("Settings")]
    [SerializeField] public float groundedAcceleration = 0.125f;
    [SerializeField] public float groundedDeceleration = 0.125f;
    [SerializeField] public float airborneAcceleration = 0.3f;
    [SerializeField] public float airborneDeceleration = 0.3f;
    
    private Vector3 CurrentVelocity => MovementSystem.Velocity;
    private bool IsAccelerating => _targetVelocity != Vector3.zero;
    
    private Vector3 _targetVelocity;
    
    public override Vector3 CalculateVelocity()
    {
        _targetVelocity = MovementSystem.MovementDirection * MovementSystem.CurrentMaxSpeed;
        
        ClampVelocity();
        
        float currentGroundedAcceleration = IsAccelerating
            ? CalculateAcceleration(groundedAcceleration)
            : CalculateAcceleration(groundedDeceleration); 
        
        float currentAirborneAcceleration = IsAccelerating
            ? CalculateAcceleration(airborneAcceleration) 
            : CalculateAcceleration(airborneDeceleration);
        
        float acceleration = MovementSystem.IsGrounded ? currentGroundedAcceleration : currentAirborneAcceleration;
        
        return Vector3.MoveTowards(CurrentVelocity, _targetVelocity, acceleration);
    }

    private void ClampVelocity()
    {
        if (_targetVelocity.x > 0)
            PositiveClampVelocity();

        if (_targetVelocity.z < 0)
            NegativeClampVelocity();
    }

    private void PositiveClampVelocity()
    {
        if (_targetVelocity.x >= MovementSystem.CurrentMaxSpeed)
            _targetVelocity.x = CurrentVelocity.x;
        
        if (_targetVelocity.z >= MovementSystem.CurrentMaxSpeed)
            _targetVelocity.z = CurrentVelocity.z;
    }
    
    private void NegativeClampVelocity()
    {
        if (_targetVelocity.x <= -MovementSystem.CurrentMaxSpeed)
            _targetVelocity.x = CurrentVelocity.x;
        
        if (_targetVelocity.z <= -MovementSystem.CurrentMaxSpeed)
            _targetVelocity.z = CurrentVelocity.z;
    }
    
    private float CalculateAcceleration(float acceleration)
    {
        return 1 / acceleration * MovementSystem.CurrentMaxSpeed * Time.deltaTime;
    }
}