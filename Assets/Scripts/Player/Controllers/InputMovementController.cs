using UnityEngine;

public class InputMovementController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backwardKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;

    [Header("Dependencies")] 
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private Transform lookDirection;

    private void Update()
    {
        movementSystem.MovementDirection = GetMovementDirection();
    }
    
    private Vector3 GetMovementDirection()
    {
        float forwardAxis = CalculateAxis(forwardKey, backwardKey);
        float rightAxis = CalculateAxis(rightKey, leftKey);

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