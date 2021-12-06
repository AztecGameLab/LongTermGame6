using UnityEngine;

public class InputJumpingController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Controls controls;
    [SerializeField] private JumpingSystem jumpingSystem;

    private void Update()
    {
        jumpingSystem.HoldingJump = Input.GetKey(controls.jumpKey);
    }
}