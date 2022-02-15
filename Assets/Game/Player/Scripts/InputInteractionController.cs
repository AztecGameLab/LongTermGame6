using UnityEngine;

/// <summary>
/// Controls the interaction system with input. 
/// </summary>

public class InputInteractionController : InputController<InteractionSystem>
{
    //METHODS FOR SCROLLWHEEL MOVEMENT
    [SerializeField]
    [Tooltip("Control the scrollWheel speed to objects")]
    private float _scaleScrollWheelSpeed;

    [SerializeField]
    [Tooltip("Distance between interactables and player when using scrollwheel")]
    private float _interactableDistance;

    private void Awake()
    {
        _scaleScrollWheelSpeed = 0.5f;
        _interactableDistance = 0.75f;
    }

    private void Update()
    {
        system.HoldingInteract = Input.GetKey(controls.interact);

        if (system.HasInteractable && system.CurrentMovableObject.GetComponent<Rigidbody>() != null)
            ScrollWheelController();
    }

    private void ScrollWheelController()
    {
        float currentScrollSpeed = Input.GetAxis("Mouse ScrollWheel");

        if (currentScrollSpeed != 0)
        {
            Vector3 normalizedDirection = (system.CurrentMovableObject._targetTransform.position - system.cameraTransform.position).normalized;
            float CurrentDistance = Vector3.Distance(system.CurrentMovableObject.GetComponent<Rigidbody>().ClosestPointOnBounds(system.cameraTransform.position), system.cameraTransform.position);

            if (currentScrollSpeed > 0 && CurrentDistance < system.CurrentInteractable.InteractRange) //forward : make sure not to pull extra use the InteractRange
            {
                system.CurrentMovableObject._targetTransform.position += (normalizedDirection * currentScrollSpeed * _scaleScrollWheelSpeed);
            }
            if (currentScrollSpeed < 0 && CurrentDistance > _interactableDistance) // backwards : make sure cant pull towards the player
            {
                system.CurrentMovableObject._targetTransform.position += (normalizedDirection * currentScrollSpeed * _scaleScrollWheelSpeed);
            }
        }
    }

    private void OnDisable()
    {
        system.HoldingInteract = false;
    }
}