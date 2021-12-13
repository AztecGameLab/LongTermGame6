using UnityEngine;

// todo: use state-machine so the code logic makes more sense.

public class FPSController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private ControlSettings controlSettings;
    [SerializeField] private Transform playerYaw;
    [SerializeField] private GroundedCheck groundCheck;
    
    [Space]
    [SerializeField] private CrouchSystem crouchSystem;
    [SerializeField] private JumpingSystem jumpingSystem;
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private RotationSystem rotationSystem;

    private Buffer _jumpBuffer = new Buffer();

    private void OnEnable()
    {
        jumpingSystem.onJump.AddListener(ClearBuffer);
    }

    private void OnDisable()
    {
        jumpingSystem.onJump.RemoveListener(ClearBuffer);
    }

    private void ClearBuffer()
    {
        _jumpBuffer.Clear();
    }

    private void Update()
    {
        // Not in any particular order right now, I don't think the timing matters (maybe in the future).
     
        UpdateCrouch();
        UpdateJump();
        UpdateMovement();
        UpdateRotation();
    }

    private void UpdateCrouch()
    {
        bool wantsToCrouch = Input.GetKey(controlSettings.crouchKey);
        
        if (groundCheck.IsGrounded || !wantsToCrouch)
            crouchSystem.WantsToCrouch = wantsToCrouch;
    }

    private void UpdateJump()
    {
        bool wantsToJump = jumpingSystem.JumpSettings.HoldAndJump
            ? Input.GetKey(controlSettings.jumpKey)
            : Input.GetKeyDown(controlSettings.jumpKey);
        
        if (wantsToJump)
            _jumpBuffer.Queue();

        jumpingSystem.WantsToJump = !crouchSystem.IsCrouching && _jumpBuffer.IsQueued(jumpingSystem.JumpSettings.JumpBufferTime);
        jumpingSystem.HoldingJump = Input.GetKey(controlSettings.jumpKey);
    }

    private void UpdateRotation()
    {
        float yMultiplier = controlSettings.invertY ? 1 : -1;
        
        rotationSystem.Pitch += Input.GetAxisRaw("Mouse Y") * yMultiplier;
        rotationSystem.Yaw += Input.GetAxisRaw("Mouse X") * controlSettings.mouseSensitivity;
    }
    
    private void UpdateMovement()
    {
        movementSystem.MovementDirection = GetMovementDirection();
    }
    
    private Vector3 GetMovementDirection()
    {
        float forwardAxis = CalculateAxis(controlSettings.forwardKey, controlSettings.backwardKey);
        float rightAxis = CalculateAxis(controlSettings.rightKey, controlSettings.leftKey);

        Vector3 forward = playerYaw.forward * forwardAxis;
        Vector3 right = playerYaw.right * rightAxis;

        return (forward + right).normalized;
    }

    private static float CalculateAxis(KeyCode positive, KeyCode negative)
    {
        float result = 0;
        
        if (Input.GetKey(positive))
            result += 1;
            
        if (Input.GetKey(negative))
            result -= 1;

        return result;
    }
}