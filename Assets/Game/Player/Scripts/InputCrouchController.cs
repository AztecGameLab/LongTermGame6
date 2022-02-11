using UnityEngine;

namespace Player.Crouching
{
    /// <summary>
    /// Controls the crouching system with input.
    /// </summary>
    
    public class InputCrouchController : InputController<CrouchSystem>
    {
        private void Update()
        {
            system.WantsToCrouch = Input.GetKey(controls.sneak);
        }

        private void OnDisable()
        {
            system.WantsToCrouch = false;
        }
    }
}