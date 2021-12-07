using UnityEngine;

// todo: use state-machine so the movement makes more sense.

// e.g. only crouch while grounded, ect. 

public class FPSController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Controls controls;
    [SerializeField] private Transform playerYaw;
    [SerializeField] private GroundedCheck groundCheck;
    
    [Space]
    [SerializeField] private CrouchSystem crouchSystem;
    [SerializeField] private JumpingSystem jumpingSystem;
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private RotationSystem rotationSystem;

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
        bool wantsToCrouch = Input.GetKey(controls.crouchKey);
        
        if (groundCheck.IsGrounded || !wantsToCrouch)
            crouchSystem.WantsToCrouch = wantsToCrouch;
    }

    private void UpdateJump()
    {
        jumpingSystem.HoldingJump = !crouchSystem.IsCrouching && Input.GetKey(controls.jumpKey);
    }

    private void UpdateRotation()
    {
        float yMultiplier = controls.invertY ? 1 : -1;
        
        rotationSystem.Pitch += Input.GetAxisRaw("Mouse Y") * yMultiplier;
        rotationSystem.Yaw += Input.GetAxisRaw("Mouse X") * controls.mouseSensitivity;
    }
    
    private void UpdateMovement()
    {
        movementSystem.MovementDirection = GetMovementDirection();
    }
    
    private Vector3 GetMovementDirection()
    {
        float forwardAxis = CalculateAxis(controls.forwardKey, controls.backwardKey);
        float rightAxis = CalculateAxis(controls.rightKey, controls.leftKey);

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