using UnityEngine;

public class InputJumpingController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private KeyCode jumpButton = KeyCode.Space;
    [SerializeField] private bool scrollToJump = true;
    
    [Header("Dependencies")] 
    [SerializeField] private JumpingSystem jumpingSystem;

    private void Update()
    {
        bool holdingJump = Input.GetKey(jumpButton);
        bool scrollingJump = scrollToJump && Input.mouseScrollDelta != Vector2.zero;

        jumpingSystem.HoldingJump = holdingJump || scrollingJump;
    }
}