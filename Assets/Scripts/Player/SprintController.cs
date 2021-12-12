using UnityEngine;
using UnityEngine.Events;

// todo: add a sprint / stamina resource to punish infinite sprinting?

/// <summary>
/// Changes the forward speed of a MovementSystem based on a multiplier.
/// </summary>

public class SprintController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float sprintMultiplier = 1f;
    [SerializeField] private bool showDebug;

    [Header("Dependencies")] 
    [SerializeField] private ControlSettings controls;
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private CrouchSystem crouchSystem;
    
    [Space(20)]
    [SerializeField] private UnityEvent onStartSprint;
    [SerializeField] private UnityEvent onStopSprint;

    private bool _isSprinting;
    private bool _wasSprinting;
    
    private bool JustStartedSprinting => !_wasSprinting && _isSprinting;
    private bool JustStoppedSprinting => _wasSprinting && !_isSprinting;
    
    private void Update()
    {
        _isSprinting = CheckIfSprinting(); 
        
        UpdateSpeed();
        UpdateEvents();
        
        if (showDebug)
            UpdateDebug();

        _wasSprinting = _isSprinting;
    }

    private bool CheckIfSprinting()
    {
        return !crouchSystem.IsCrouching && Input.GetKey(controls.sprintKey);
    }

    private void UpdateSpeed()
    {
        if (JustStartedSprinting)
            movementSystem.ForwardSpeedMultiplier = sprintMultiplier;

        if (JustStoppedSprinting)
            movementSystem.ForwardSpeedMultiplier = 1 / sprintMultiplier;
    }

    private void UpdateEvents()
    {
        if (JustStartedSprinting)
            onStopSprint.Invoke();
        
        if (JustStoppedSprinting)
            onStartSprint.Invoke();
    }

    #region Debug

    private void UpdateDebug()
    {
        if (JustStartedSprinting)
            Debug.Log("Just started sprinting!");
        
        if (JustStoppedSprinting)
            Debug.Log("Just stopped sprinting!");
    }

    private void OnGUI()
    {
        if (showDebug)
            GUILayout.Label($"Is Sprinting: {_isSprinting}");
    }

    #endregion
}