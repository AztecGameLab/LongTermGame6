using UnityEngine;

public class InputRotationController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Controls controls;
    [SerializeField] private RotationSystem rotationSystem;
    
    private void Update()
    {
        rotationSystem.Pitch += Input.GetAxisRaw("Mouse Y") * (controls.invertY ? 1 : -1);
        rotationSystem.Yaw += Input.GetAxisRaw("Mouse X") * controls.mouseSensitivity;
    }
}