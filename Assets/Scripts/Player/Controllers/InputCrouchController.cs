using UnityEngine;

public class InputCrouchController : MonoBehaviour
{

    [Header("Dependencies")] 
    [SerializeField] private Controls controls;
    [SerializeField] private CrouchSystem crouchSystem;

    private void Update()
    {
        crouchSystem.WantsToCrouch = Input.GetKey(controls.crouchKey);
    }
}