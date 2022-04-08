using UnityEngine;

/// <summary>
/// Controls the movement system with input.
/// </summary>

public class InputMovementController : InputController<MovementSystem>
{
    [Header("Dependencies")]
    
    [SerializeField]
    [Tooltip("The transform that points in the forward movement direction.")]
    private Transform playerYaw;
    
    [SerializeField]
    [Tooltip("The crouch system used for determining crouch walk.")]
    private CrouchSystem crouchSystem;

    private bool _isInstanceNotNull;

    private void Start()
    {
        _isInstanceNotNull = HearingManager.Instance != null;
    }

    private void Update()
    {
        float forwardAxis = CalculateAxis(controls.forward, controls.backward);
        float rightAxis = CalculateAxis(controls.right, controls.left);

        Vector3 forward = playerYaw.forward * forwardAxis;
        Vector3 right = playerYaw.right * rightAxis;

        system.MovementDirection = (forward + right).normalized;
    }
    
    private float CalculateAxis(KeyCode positive, KeyCode negative)
    {
        float result = 0;

        if (Input.GetKey(positive))
        {
            result += 1;
            OnWalkEmitSound();
        }

        if (Input.GetKey(negative))
        {
            result -= 1;
            OnWalkEmitSound();
        }

        return result;
    }

    private  void OnWalkEmitSound()
    {
        if(!crouchSystem.IsCrouching && _isInstanceNotNull)
            HearingManager.Instance.OnSoundEmitted(gameObject, transform.position, EHeardSoundCategory.EFootstep, .5f);
    }

    private void OnDisable()
    {
        system.MovementDirection = Vector3.zero;
        system.Rigidbody.velocity = Vector3.zero;
    }
}