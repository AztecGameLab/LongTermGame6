using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight  : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Change light Flicker randomness")]
    [Range(0.1f, 0.9f)]
    private float _randomFactorFlicker = 0.5f;
    [SerializeField] 
    [Tooltip("Change light Color randomness")]
    [Range(0.1f, 0.9f)]
    private float _randomFactorColor = 0.5f;
    
    private GradientColorKey[] colorKey;
    private GradientAlphaKey[] alphaKey;
    private Gradient _gradient;
    
    private void Update()
    {
        Light myLight = GetComponent<Light>();
        float noise = Mathf.PerlinNoise(Time.time * _randomFactorFlicker, 0.0f);
        Debug.Log("Current Noise: " + noise);
        myLight.intensity = (float) (1 - (0.5 * noise));
        myLight.color = GetColor();

    }

    private Color GetColor()
    {
        _gradient = new Gradient();
        
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = new Color(0.92f, 0.27f, 0.07f);
        colorKey[0].time = 0.0f;
        colorKey[1].color = new Color(0.69f, 0.18f, 0.05f);
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;
        
        _gradient.SetKeys(colorKey, alphaKey);
        return _gradient.Evaluate((float) (1 - 0.5 * Mathf.PerlinNoise(Time.time * _randomFactorColor, 0.0f)));
    }
}
