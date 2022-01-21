using UnityEngine;

/// <summary>
/// Controls the movement system with input.
/// </summary>

public class InputMovementController : InputController<MovementSystem>
{
    [Header("Dependencies")]
    
    [SerializeField] 
    [Tooltip("The transform that points in the forward movement direction.")]
    private Transform playerYaw;

    private void Update()
    {
        float forwardAxis = CalculateAxis(controls.forward, controls.backward);
        float rightAxis = CalculateAxis(controls.right, controls.left);

        Vector3 forward = playerYaw.forward * forwardAxis;
        Vector3 right = playerYaw.right * rightAxis;

        system.UpdateMovement((forward + right).normalized);
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