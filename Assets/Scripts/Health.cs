using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls changing and regenerating the health of an object over time.
/// </summary>

// todo: consider moving away from a percent based system, while retaining a "percent health" field to read from

public class Health : MonoBehaviour
{
    [Header("Settings")]
    
    [SerializeField] 
    [Tooltip("How much health we currently have. This is updated during gameplay.")]
    private float currentHealthPercent = 1f;
    
    [SerializeField] 
    [Tooltip("How long it takes to recover 100% of your health, in seconds.")]
    private float healthRecoverySpeed = 1f;
    
    [Space(20f)]
    
    [SerializeField] 
    private UnityEvent<float> onHealthChanged;
    
    [SerializeField] 
    private UnityEvent onDamage;

    [SerializeField] 
    private UnityEvent onDeath;
    
    // Methods
    
    private void Update()
    {
        RecoverHealth();
    }

    private void RecoverHealth()
    {
        float current = currentHealthPercent;
        float maxDelta = healthRecoverySpeed * Time.deltaTime;
        float targetHealth = Mathf.MoveTowards(current, 1, maxDelta);

        SetHealthPercent(targetHealth);
    }

    private void SetHealthPercent(float percent)
    {
        currentHealthPercent = percent;
        onHealthChanged.Invoke(currentHealthPercent);
        
        if (currentHealthPercent <= 0)
            onDeath.Invoke();
    }

    /// <summary>
    /// Take away a percent of this object's health.
    /// <remarks>The input will always be clamped between 0 and 1.</remarks>
    /// </summary>
    /// <param name="percent">What percent we should take away.</param>
    public void Damage(float percent)
    {
        float clampedPercent = Mathf.Clamp01(currentHealthPercent - percent);

        SetHealthPercent(clampedPercent);
        onDamage.Invoke();
    }
}