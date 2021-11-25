using UnityEngine;

public class InputInteractionController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private KeyCode interactButton = KeyCode.E;
    
    [Header("Dependencies")] 
    [SerializeField] private InteractionSystem interactionSystem;

    private void Update()
    {
        interactionSystem.HoldingInteract = Input.GetKey(interactButton);
    }
}