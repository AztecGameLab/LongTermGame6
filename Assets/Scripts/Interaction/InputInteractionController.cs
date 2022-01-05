using UnityEngine;

public class InputInteractionController : InputController<InteractionSystem>
{
    private void Update()
    {
        system.HoldingInteract = Input.GetKey(controls.interact);
    }
}