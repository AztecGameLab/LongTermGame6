using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/EntitySoundProfile", order = 1)]
public class EntitySoundProfile : ScriptableObject
{
    [System.Serializable]
    public struct Surface {
        [NaughtyAttributes.Tag]
        public string surfaceTag;
        [EventRef]
        public string stepSound;
        [EventRef]
        public string crouchedStepSound;
        [EventRef]
        public string jumpSound;
        [EventRef] 
        public string landSound;
        [SerializeField]
        public bool isWet;
    }

    [SerializeField]
    private Surface defaultSurface;
    
    [SerializeField]
    private List<Surface> surfaces;

    public Surface FindSurface(string tag)
    {
        foreach (Surface surface in surfaces)
        {
            if (tag == surface.surfaceTag)
                return surface;
        }

        return defaultSurface;
    }
}