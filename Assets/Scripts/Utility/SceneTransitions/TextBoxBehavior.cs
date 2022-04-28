using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxBehavior : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Image textBoxImage;

    [Header("Fading Settings")] 
    [SerializeField] private Color colorTextBox;
    [SerializeField] [Range(0f, 0.5f)] private float fadingSpeed;
    [SerializeField] [Range(0f, 1f)] private float alphaMin;
    [SerializeField] [Range(0f, 1f)] private float alphaMax;
    [SerializeField] [Range(0f, 0.5f)]private float alphaTolerance;
    
    

    public IEnumerator FadeOutOnDisable()
    {
        if (Math.Abs(textBoxImage.color.a - alphaMin) < alphaTolerance)
        {
            yield break;
        }
        
        for (float i = alphaMax; i >= alphaMin; i -= fadingSpeed)
        {
            textBoxImage.color = new Color(colorTextBox.r, colorTextBox.g, colorTextBox.b, i);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator FadeInOnEnable()
    {
        if (Math.Abs(textBoxImage.color.a - alphaMax) < alphaTolerance)
        {
            yield break;
        }
        
        for (float i = 0; i <= alphaMax; i += fadingSpeed)
        {
            textBoxImage.color = new Color(colorTextBox.r, colorTextBox.g, colorTextBox.b, i);
            yield return new WaitForEndOfFrame();
        }
    }
}
