using UnityEngine;

/// <summary>
/// Animates a UI element based on the current state of an interaction system.
/// </summary>

public class CrosshairAnimator : MonoBehaviour
{
    [Header("Settings")]
    
    [SerializeField]
    [Tooltip("Crosshair scale when interacting with something.")]
    private Vector3 interactableScale;
    
    [SerializeField]
    [Tooltip("Default crosshair scale.")]
    private Vector3 normalScale;
    
    [SerializeField]
    [Tooltip("How long it should take to animate the crosshair scale, in seconds.")]
    private float animationSpeed;
    
    [SerializeField]
    [Tooltip("How long it should take to animate the crosshair alpha, in seconds.")]
    private float fadeSpeed = 1f;

    [Header("Dependencies")]
    
    [SerializeField]
    [Tooltip("Used to check if we are holding anything.")]
    private InteractionSystem interactionSystem;
    
    [SerializeField]
    [Tooltip("The UI element that is faded in or out depending on our interaction state.")]
    private CanvasGroup crosshair;
    
    // Methods

    private void Update()
    {
        UpdateCrosshairAlpha();
        UpdateCrosshairSize();
    }

    private void UpdateCrosshairAlpha()
    {
        float current = crosshair.alpha;
        float target = interactionSystem.CurrentInteractable != null ? 0 : 1;

        float maxDistanceDelta = fadeSpeed * Time.deltaTime;

        crosshair.alpha = Mathf.MoveTowards(current, target, maxDistanceDelta);
    }

    private void UpdateCrosshairSize()
    {
        Vector3 current = crosshair.transform.localScale;
        Vector3 target = interactionSystem.LookingAtInteractable ? interactableScale : normalScale;
        
        float maxDistanceDelta = animationSpeed * Time.deltaTime;
        
        crosshair.transform.localScale = Vector3.MoveTowards(current, target, maxDistanceDelta);
    }
}