using System;
using System.Collections;
using UnityEngine;
using Display = UnityTemplateProjects.UI.Display;

namespace Game.Interaction
{
    public class Hint : MonoBehaviour
    {
        [SerializeField] private string text;
        [SerializeField] private float fadeSpeed = 1f;
        [SerializeField] private float hintDuration = 4f;
        
        [Header("Dependencies")]
        [SerializeField] private Display textDisplay;
        [SerializeField] private CanvasGroup textCanvas;

        private void Start()
        {
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
                textCanvas.alpha = Mathf.MoveTowards(textCanvas.alpha, target, fadeSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator AutoHide()
        {
            yield return new WaitForSeconds(hintDuration);
            HideHint();
        }
    }
}