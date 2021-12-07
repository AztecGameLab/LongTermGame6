using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private ControlSettings controlSettings;
    [SerializeField] private InteractionSystem interactionSystem;

    private void Update()
    {
        interactionSystem.HoldingInteract = Input.GetKey(controlSettings.interactKey);
    }
}