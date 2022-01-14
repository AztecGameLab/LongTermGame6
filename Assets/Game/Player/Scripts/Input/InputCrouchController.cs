using UnityEngine;

namespace Player.Crouching
{
    public class InputCrouchController : InputController<CrouchSystem>
    {
        [SerializeField] private GroundCheck groundCheck;
        
        private void Update()
        {
            bool wantsToCrouch = Input.GetKey(controls.sneak);
        
            if (groundCheck.IsGrounded || !wantsToCrouch)
                system.WantsToCrouch = wantsToCrouch;
        }
    }
}