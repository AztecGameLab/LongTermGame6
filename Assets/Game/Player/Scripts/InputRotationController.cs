using UnityEngine;

/// <summary>
/// Controls the rotation system with input.
/// </summary>

public class InputRotationController : InputController<RotationSystem>
{
    private void Update()
    {
        float yMultiplier = controls.invertY ? 1 : -1;

        //added Time.timeScale to prevent movement in pause menu
        system.Pitch += Input.GetAxisRaw("Mouse Y") * yMultiplier * Time.timeScale;
        system.Yaw += Input.GetAxisRaw("Mouse X") * controls.sensitivity * Time.timeScale;
    }
}