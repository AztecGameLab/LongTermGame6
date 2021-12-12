using UnityEngine;

[CreateAssetMenu]
public class ControlSettings : ScriptableObject
{
    [Header("Walking")]
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    
    [Header("Abilities")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode interactKey = KeyCode.Mouse0;
    public KeyCode jumpKey = KeyCode.Space;
    
    [Header("Mouse")]
    public bool invertY;
    public float mouseSensitivity = 1f;
}