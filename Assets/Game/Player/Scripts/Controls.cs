using UnityEngine;

[CreateAssetMenu]
public class Controls : ScriptableObject
{
    [Header("Mouse Settings")] 
    [SerializeField] public float sensitivity = 1f;
    [SerializeField] public bool invertY;
    
    [Header("Movement Keybinds")]
    [SerializeField] public KeyCode forward = KeyCode.W;
    [SerializeField] public KeyCode backward = KeyCode.S;
    [SerializeField] public KeyCode left = KeyCode.A;
    [SerializeField] public KeyCode right = KeyCode.D;
    
    [Header("Special Keybinds")]
    [SerializeField] public KeyCode jump = KeyCode.Space;
    [SerializeField] public KeyCode sneak = KeyCode.LeftControl;
    [SerializeField] public KeyCode sprint = KeyCode.LeftShift;
    [SerializeField] public KeyCode interact = KeyCode.Mouse0;
    [SerializeField] public KeyCode pause = KeyCode.Escape;
    [SerializeField] public KeyCode leanLeft = KeyCode.Q;
    [SerializeField] public KeyCode leanRight = KeyCode.E;

    [Header("Ability Keybinds")] 
    [SerializeField] public KeyCode throwAbility = KeyCode.Mouse1;
    [SerializeField] public KeyCode primary = KeyCode.Mouse0;
    [SerializeField] public KeyCode secondary = KeyCode.Mouse1;
}