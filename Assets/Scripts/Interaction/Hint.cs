using System;
using System.Collections;
using UnityEngine;
using Display = UnityTemplateProjects.UI.Display;

namespace Game.Interaction
{
    public class Hint : MonoBehaviour
    {
        [Multiline]
        [SerializeField]
        [Tooltip("The text that this hint will display.")]
        private string text;
        
        [Header("Settings")]

        [SerializeField] 
        [Tooltip("How quickly the hint should fade in / out.")]
        private float fadeSpeed = 1f;
        
        [SerializeField] 
        [Tooltip("How long the hint should display before automatically fading out.")]
        private float hintDuration = 4f;
        
        [Header("Dependencies")]

        [SerializeField]
        [Tooltip("The display used for showing the hint.")]
        private Display textDisplay;
        
        [SerializeField] 
        [Tooltip("The canvas used for fading the hint in and out.")]
        private CanvasGroup textCanvas;
        
        private WaitForSeconds _autoHideDuration;
        
        private void Start()
        {
            _autoHideDuration = new WaitForSeconds(hintDuration);
            textDisplay.UpdateText(text);
        }

        public void ShowHint()
        {
            StopAllCoroutines();
            StartCoroutine(FadeAnimation(1));
            StartCoroutine(AutoHide());
        }

        public void HideHint()
        {
            StopAllCoroutines();
            StartCoroutine(FadeAnimation(0));
        }
        
        private IEnumerator FadeAnimation(float target)
        {
            while (Math.Abs(textCanvas.alpha - target) > 0)
            {
                float current = textCanvas.alpha;
                float maxDelta = fadeSpeed * Time.deltaTime;
                
                textCanvas.alpha = Mathf.MoveTowards(current, target, maxDelta);
                
                yield return null;
            }
        }

        private IEnumerator AutoHide()
        {
            yield return _autoHideDuration;
            HideHint();
        }
    }
}