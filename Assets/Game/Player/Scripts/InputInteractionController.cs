using UnityEngine;

/// <summary>
/// Controls the interaction system with input. 
/// </summary>

public class InputInteractionController : InputController<InteractionSystem>
{
    [SerializeField]
    [Tooltip("Scroll wheel speed")]
    private float scrollWheelSpeed;

    [SerializeField]
    [Tooltip("Distance between player and gameobject")]
    private float interactableDistance;

    private Transform _camaraTransform;


    private void Awake()
    {
        _camaraTransform = Camera.main.transform != null? Camera.main.transform: transform;
        scrollWheelSpeed = 0.5f;
        interactableDistance = 0.75f;
    }

    private void Update()
    {
        system.HoldingInteract = Input.GetKey(controls.interact);

        if (system.HoldingInteract )//&& system.CurrentInteractable.GetComponent<Rigidbody>() != null)
            scrollWheelController();
    }

    private void OnDisable()
    {
        system.HoldingInteract = false;
    }

    private void scrollWheelController()
    {
        float currentScrollSpeed = Input.GetAxis("Mouse ScrollWheel");

        if( currentScrollSpeed != 0 && !system.CurrentMovableObject.hasCollision )
        {
            Vector3 normalizedDirection = (system.CurrentMovableObject._targetTransform.position - _camaraTransform.position ).normalized;
            Vector3 distanceAdded = system.CurrentMovableObject._targetTransform.position + normalizedDirection * currentScrollSpeed * scrollWheelSpeed;
            float distanceFromObject = Vector3.Distance(distanceAdded, _camaraTransform.position);

            if (currentScrollSpeed > 0 && distanceFromObject < system.CurrentInteractable.InteractRange )
            {
                system.CurrentMovableObject._targetTransform.position += normalizedDirection * currentScrollSpeed * scrollWheelSpeed;
            }
            if (currentScrollSpeed < 0 && distanceFromObject > interactableDistance )
            {
                system.CurrentMovableObject._targetTransform.position += normalizedDirection * currentScrollSpeed * scrollWheelSpeed;
            }
        }
    }
}