using System;
using System.Collections;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public abstract class TextPlayer : MonoBehaviour
    {
        [Serializable]
        
        public struct TextData
        {
            [ResizableTextArea]
            public string value;
            
            [MinValue(0)]
            [AllowNesting]    
            public float duration;

            public UnityEvent onShow, onHide;
        }

        [SerializeField] 
        private TextData[] textData;

        [SerializeField] private bool playOnStart;

        private void Start()
        {
            if (playOnStart)
                StartPlaying();
        }

        [PublicAPI]
        public void StartPlaying()
        {
            StartCoroutine(Animation());
        }

        [PublicAPI]
        public void StopPlaying()
        {
            StopCoroutine(Animation());
        }

        private IEnumerator Animation()
        {
            // return textData
            //     .Select(DisplayData)
            //     .GetEnumerator();

            foreach (TextData data in textData)
            {
                data.onShow.Invoke();
                yield return DisplayData(data);
                data.onHide.Invoke();
            }
        }
        
        protected abstract IEnumerator DisplayData(TextData data);
    }
}