using UnityEngine;

public class InputCrouchController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private KeyCode crouchButton = KeyCode.LeftShift;

    [Header("Dependencies")] 
    [SerializeField] private CrouchSystem crouchSystem;

    private void Update()
    {
        crouchSystem.WantsToCrouch = Input.GetKey(crouchButton);
    }
}