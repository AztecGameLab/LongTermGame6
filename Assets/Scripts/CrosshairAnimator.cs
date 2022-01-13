using UnityEngine;

public class CrosshairAnimator : MonoBehaviour
{
    [SerializeField] private InteractionSystem interactionSystem;
    [SerializeField] private CanvasGroup crosshair;
    [SerializeField] private Vector3 interactableScale;
    [SerializeField] private Vector3 normalScale;
    [SerializeField] private float animationSpeed;

    [SerializeField] private float fadeSpeed = 1f;
    
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