using UnityEngine;

public class FlickeringLight  : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The light to apply the flickering to.")]
    private Light target;
    
    [SerializeField] 
    [Tooltip("Change light Flicker randomness")]
    private float randomFactorFlicker = 0.5f;
    
    [SerializeField] 
    [Tooltip("Change light Color randomness")]
    private float randomFactorColor = 0.5f;

    [SerializeField]
    private float minIntensity = 0.1f;
    
    [SerializeField]
    private float maxIntensity = 0.5f;
    
    [SerializeField] 
    private Gradient colorGradient;
    
    private void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * randomFactorFlicker, 0.0f);
        target.color = GetColor();
        target.intensity = minIntensity + (maxIntensity - minIntensity) * noise;
    }

    private Color GetColor()
    {
        return colorGradient.Evaluate(Mathf.PerlinNoise(Time.time * randomFactorColor, 0.0f));
    }
}
