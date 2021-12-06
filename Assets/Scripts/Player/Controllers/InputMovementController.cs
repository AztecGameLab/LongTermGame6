using UnityEngine;

public class InputMovementController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Controls controls;
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private Transform lookDirection;

    private void Update()
    {
        movementSystem.MovementDirection = GetMovementDirection();
    }
    
    private Vector3 GetMovementDirection()
    {
        float forwardAxis = CalculateAxis(controls.forwardKey, controls.backwardKey);
        float rightAxis = CalculateAxis(controls.rightKey, controls.leftKey);

        Vector3 forward = lookDirection.forward * forwardAxis;
        Vector3 right = lookDirection.right * rightAxis;

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