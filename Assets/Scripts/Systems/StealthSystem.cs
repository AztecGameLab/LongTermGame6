using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    public float crounchedValue = 0.5f;

    float _stealthValue;

    public void MakeUndetectable()
    {
        this._stealthValue = 0.0f;
    }

    public void SetCrouched()
    {
        this._stealthValue = crounchedValue;
    }

    public void resetStealthValue()
    {
        this._stealthValue = 0.0f;
    }
}
