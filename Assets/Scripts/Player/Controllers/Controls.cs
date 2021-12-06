using UnityEngine;

[CreateAssetMenu]
public class Controls : ScriptableObject
{
    [Header("Walking")]
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    
    [Header("Abilities")]
    public KeyCode crouchKey = KeyCode.LeftShift;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode jumpKey = KeyCode.Space;
    
    [Header("Mouse")]
    public bool invertY;
    public float mouseSensitivity = 1f;
}