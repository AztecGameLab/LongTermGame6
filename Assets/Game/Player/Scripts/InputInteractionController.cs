using UnityEngine;

/// <summary>
/// Controls the interaction system with input. 
/// </summary>

public class InputInteractionController : InputController<InteractionSystem>
{
    private void Update()
    {
        system.HoldingInteract = Input.GetKey(controls.interact);
    }
}