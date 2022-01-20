using UnityEngine;

public class InputRotationController : InputController<RotationSystem>
{
    private void Update()
    {
        if (IsRunning == false)
            return;
        
        float yMultiplier = controls.invertY ? 1 : -1;
        
        system.Pitch += Input.GetAxisRaw("Mouse Y") * yMultiplier;
        system.Yaw += Input.GetAxisRaw("Mouse X") * controls.sensitivity;
    }
}