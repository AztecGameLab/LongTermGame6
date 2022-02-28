using UnityEngine;

namespace Player.Crouching
{
    /// <summary>
    /// Controls the crouching system with input.
    /// </summary>
    
    public class InputCrouchController : InputController<CrouchSystem>
    {
        [Header("Dependencies")]
        
        [SerializeField]
        [Tooltip("We can only crouch on the ground - this object tells us if that happens.")]
        private GroundCheck groundCheck;
        
        private void Update()
        {
            bool wantsToCrouch = Input.GetKey(controls.sneak);
        
            if (groundCheck.IsGrounded || !wantsToCrouch)
                system.WantsToCrouch = wantsToCrouch;
        }

        private void OnDisable()
        {
            system.WantsToCrouch = false;
        }
    }
}