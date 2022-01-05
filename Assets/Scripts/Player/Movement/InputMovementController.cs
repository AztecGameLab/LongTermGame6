using UnityEngine;

public class InputMovementController : InputController<MovementSystem>
{
    [SerializeField] private Transform playerYaw;

    private void Update()
    {
        float forwardAxis = CalculateAxis(controls.forward, controls.backward);
        float rightAxis = CalculateAxis(controls.right, controls.left);

        Vector3 forward = playerYaw.forward * forwardAxis;
        Vector3 right = playerYaw.right * rightAxis;

        system.UpdateMovement(IsRunning ? (forward + right).normalized : Vector3.zero);
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