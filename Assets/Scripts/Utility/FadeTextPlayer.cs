using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Utility
{
    public class FadeTextPlayer : TextPlayer
    {
        [SerializeField] 
        private TMP_Text text;

        [SerializeField]
        [MinValue(0)]
        private float introTime, outroTime;
        
        [SerializeField] 
        private CanvasGroup canvasGroup;
        
        protected override IEnumerator DisplayData(TextData data)
        {
            text.text = data.value;
            
            float elapsedTime = 0;

            while (elapsedTime < introTime)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / introTime);
                yield return null;
            }

            yield return new WaitForSeconds(data.duration);

            elapsedTime = 0;
            
            while (elapsedTime < outroTime)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / outroTime);
                yield return null;
            }
        }
    }
}