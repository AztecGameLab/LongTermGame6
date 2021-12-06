using UnityEngine;

public class InputInteractionController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Controls controls;
    [SerializeField] private InteractionSystem interactionSystem;

    private void Update()
    {
        interactionSystem.HoldingInteract = Input.GetKey(controls.interactKey);
    }
}