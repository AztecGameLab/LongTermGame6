using UnityEngine;

public class InputThrowController : InputController<InteractionSystem>
{
    [Header("Settings")]
    
    [SerializeField]
    [Tooltip("What is this objects max velocity when thrown?")]
    private float maxThrowSpeed;
    
    [SerializeField] 
    [Tooltip("Use this to tune how fast heavy objects are thrown.")]
    private float massFactor;
    
    private void Update()
    {
        if (Input.GetKeyDown(controls.throwAbility) && system.HasInteractable)
            Throw(system.CurrentInteractable);
    }

    private void Throw(Interactable interactable)
    {
        Drop(interactable);

        if (interactable.TryGetComponent(out Rigidbody targetRigidbody))
            ApplyThrowForce(targetRigidbody);
    }

    private static void Drop(Interactable target)
    {
        target.InteractEnd();
    }

    private void ApplyThrowForce(Rigidbody targetRigidbody)
    {
        Vector3 forceDirection = system.lookDirection.forward;
        float forceMagnitude = maxThrowSpeed / Mathf.Max(Mathf.Sqrt(targetRigidbody.mass * massFactor), 1);
        
        targetRigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.VelocityChange);
    }
}
