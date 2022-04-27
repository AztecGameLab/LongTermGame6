using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/EntitySoundProfile", order = 1)]
public class EntitySoundProfile : ScriptableObject
{
    [System.Serializable]
    private struct Surface {
        [NaughtyAttributes.Tag]
        public string surfaceTag;
        [EventRef]
        public string defaultStepSound;
        [EventRef]
        public string crouchedStepSound;
        [EventRef]
        public string jumpSound;
        [SerializeField]
        public bool isWet;
    }
    [SerializeField]
    private List<Surface> surfaces;

    /*[EventRef]
    public string crawlingSound;
    [EventRef]
    public string runningSound;*/

    public string GetSoundOnSurface(string tag)
    {
        foreach(Surface s in surfaces)
        {
            if (tag == s.surfaceTag)
                return s.defaultStepSound;
        }

        return "";
    }
}