using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    public float crounchedValue = 0.5f;

    public float stealthValue { get; set; }

    private void OnEnable()
    {
        stealthValue = 1.0f;
    }

    public void MakeUndetectable()
    {
        this.stealthValue = 0.0f;
    }

    public void SetCrouched()
    {
        this.stealthValue = crounchedValue;
    }

    public void resetStealthValue()
    {
        this.stealthValue = 1.0f;
    }
}
