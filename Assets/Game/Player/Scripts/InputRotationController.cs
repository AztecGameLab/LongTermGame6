using UnityEngine;

/// <summary>
/// Controls the rotation system with input.
/// </summary>

public class InputRotationController : InputController<RotationSystem>
{
    private void Update()
    {
        float yMultiplier = controls.invertY ? 1 : -1;

        system.Pitch += Input.GetAxisRaw("Mouse Y") * controls.sensitivity * Time.timeScale * yMultiplier;
        system.Yaw += Input.GetAxisRaw("Mouse X") * controls.sensitivity * Time.timeScale;
    }
}