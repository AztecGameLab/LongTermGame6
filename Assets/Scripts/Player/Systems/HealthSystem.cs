using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float healthRecoverySpeed = 1f;
    [SerializeField] private float healthRecoveryTolerance;
    
    [Header("Events")] 
    [SerializeField] private UnityEvent<float> onHealthChanged;
    [SerializeField] private UnityEvent onDamage;

    private float _currentHealthPercent;
    private float _lastReportedHealth;
    
    private void Update()
    {
        RecoverHealth();
        CheckHealthChangeEvent();
    }

    private void RecoverHealth()
    {
        float current = _currentHealthPercent;
        float maxDelta = healthRecoverySpeed * Time.deltaTime;
        
        _currentHealthPercent = Mathf.MoveTowards(current, 1, maxDelta);
    }

    private void CheckHealthChangeEvent()
    {
        if (Math.Abs(_currentHealthPercent - _lastReportedHealth) > healthRecoveryTolerance)
        {
            onHealthChanged.Invoke(_currentHealthPercent);
            _lastReportedHealth = _currentHealthPercent;
        }
    }
    
    public void Damage(float percent)
    {
        _currentHealthPercent = Mathf.Clamp01(_currentHealthPercent - percent);
        onDamage.Invoke();
    }
}