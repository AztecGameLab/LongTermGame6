using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

// todo: cleanup

public class JumpingSystem : MonoBehaviour
{
    [SerializeField] private JumpSettings settings;
    [SerializeField] private bool showDebug;

    [Header("Dependencies")] 
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private GroundedCheck groundCheck;
    [SerializeField] private CustomGravity customGravity;

    [Space(20)]
    [SerializeField] private UnityEvent onJump;
    
    private bool _coyoteAvailable;
    private bool _holdingJump = true;
    private bool _wasHoldingJump;
    private int _remainingAirJumps;
    private Vector3 _currentVelocity;
    private Buffer _jumpBuffer = new Buffer();
    
    private Vector3 Velocity => targetRigidbody.velocity;
    private bool IsGrounded => !groundCheck || groundCheck.IsGrounded;
    private float TimeSpentFalling => groundCheck.TimeSpentFalling;

    private bool WantsToJump => settings.HoldAndJump
        ? _holdingJump
        : _holdingJump && !_wasHoldingJump;

    [PublicAPI] public bool HoldingJump { get; set; }

    private void OnEnable()
    {
        groundCheck.CollisionEvents.onEnterCollision.AddListener(OnLand);
    }

    private void OnDisable()
    {
        groundCheck.CollisionEvents.onEnterCollision.RemoveListener(OnLand);
    }

    private void OnLand(Collider col)
    {
        _remainingAirJumps = settings.AirJumps;
        _coyoteAvailable = true;
    }

    private void Update()
    {
        _wasHoldingJump = _holdingJump;
        _holdingJump = HoldingJump;

        if (WantsToJump)
            _jumpBuffer.Queue();

        ApplyCustomGravity();
    }

    private void ApplyCustomGravity()
    {
        bool rising = Velocity.y > 0;

        if (settings.EnableFastFall && _holdingJump == false)
        {
            customGravity.gravity = rising
                ? settings.FastFallGravityRising
                : settings.FastFallGravityFalling;
        }
        else
        {
            customGravity.gravity = rising
                ? settings.StandardGravityRising
                : settings.StandardGravityFalling;
        }
    }

    private void FixedUpdate()
    {
        _currentVelocity = Velocity;

        if (ShouldJump())
            ApplyJump();

        ApplyVelocity();
    }

    private bool ShouldJump()
    {
        if (_jumpBuffer.IsQueued(settings.JumpBufferTime) == false)
            return false;

        if (IsGrounded)
            return true;

        if (_coyoteAvailable && TimeSpentFalling < settings.CoyoteTime)
            return true;

        return _remainingAirJumps > 0;
    }

    private void ApplyJump()
    {
        _jumpBuffer.Clear();

        _coyoteAvailable = false;
        _currentVelocity.y = settings.JumpSpeed;
        onJump.Invoke();
    }

    private void ApplyVelocity()
    {
        targetRigidbody.velocity = _currentVelocity;
    }

    #region Debug

    private void OnGUI()
    {
        if (showDebug)
            DrawDebugUI();
    }

    private void DrawDebugUI()
    {
        GUILayout.Label($"Holding Jump: {_holdingJump}");
        GUILayout.Label($"Current Velocity: {_currentVelocity}");
        GUILayout.Label($"Coyote Available: {_coyoteAvailable}");
        GUILayout.Label($"Jump Queued: {_jumpBuffer.IsQueued(settings.JumpBufferTime)}");
        GUILayout.Label($"Remaining Air Jumps: {_remainingAirJumps}");
    }

    #endregion
}