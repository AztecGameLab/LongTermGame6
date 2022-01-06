using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float currentHealthPercent = 1f;
    [SerializeField] private float healthRecoverySpeed = 1f;
    [SerializeField] private float healthRecoveryTolerance;
    
    [Header("Events")] 
    [SerializeField] private UnityEvent<float> onHealthChanged;
    [SerializeField] private UnityEvent onDamage;

    private float _lastReportedHealth;
    
    private void Update()
    {
        //TODO remove debug
        if (Input.GetKeyDown(KeyCode.K))
            Damage(0.25f);
        
        RecoverHealth();
        CheckHealthChangeEvent();
    }

    private void RecoverHealth()
    {
        float current = currentHealthPercent;
        float maxDelta = healthRecoverySpeed * Time.deltaTime;
        
        currentHealthPercent = Mathf.MoveTowards(current, 1, maxDelta);
    }

    private void CheckHealthChangeEvent()
    {
        if (Math.Abs(currentHealthPercent - _lastReportedHealth) > healthRecoveryTolerance)
        {
            onHealthChanged.Invoke(currentHealthPercent);
            _lastReportedHealth = currentHealthPercent;
        }
    }
    
    public void Damage(float percent)
    {
        currentHealthPercent = Mathf.Clamp01(currentHealthPercent - percent);
        onDamage.Invoke();
    }
}