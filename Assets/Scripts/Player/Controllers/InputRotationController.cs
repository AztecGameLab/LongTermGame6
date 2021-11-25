using UnityEngine;

public class InputRotationController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float rotationSpeed = 1f;
    
    [Header("Dependencies")] 
    [SerializeField] private RotationSystem rotationSystem;
    
    private void Update()
    {
        rotationSystem.Pitch += -Input.GetAxisRaw("Mouse Y");
        rotationSystem.Yaw += Input.GetAxisRaw("Mouse X") * rotationSpeed;
    }
}