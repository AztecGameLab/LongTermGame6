using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSystem : InputController<InteractionSystem>
{
    [Header("Settings")]
    [SerializeField] float maxThrowSpeed;
    [SerializeField] float massFactor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (system.CurrentInteractable.TryGetComponent<MovableObject>(out var targetMovableObject))
            {
                system.CurrentInteractable.InteractEnd();
                targetMovableObject.Rigidbody.AddForce((system.lookDirection.forward * maxThrowSpeed) / Mathf.Max(Mathf.Sqrt(targetMovableObject.Rigidbody.mass*massFactor),1), ForceMode.VelocityChange);

            }
            
        }
    }
}
