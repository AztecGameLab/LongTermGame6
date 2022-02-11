using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Changes the forward speed of a MovementSystem based on a multiplier.
/// </summary>

public class InputSprintController : InputController<MovementSystem>
{
    [Header("Settings")] 
    
    [SerializeField]
    [Tooltip("How much faster should we move while sprinting.")]
    private float sprintMultiplier = 1f;
    
    [SerializeField] 
    [Tooltip("Write debugging information to the console and screen.")]
    private bool showDebug;
    
    [Space(20)]
    
    [SerializeField]
    [Tooltip("Called when we start sprinting.")]
    private UnityEvent onStartSprint;
    
    [SerializeField] 
    [Tooltip("Called when we stop sprinting.")]
    private UnityEvent onStopSprint;

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
        return Input.GetKey(controls.sprint) && Input.GetKey(controls.forward);
    }

    private void UpdateSpeed()
    {
        if (JustStartedSprinting)
            system.ForwardSpeedMultiplier = sprintMultiplier;

        if (JustStoppedSprinting)
            system.ForwardSpeedMultiplier = 1 / sprintMultiplier;
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