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

        [SerializeField]
        public bool isWet;
    
        public EventReference stepSound;
        public EventReference crouchedStepSound;
        public EventReference jumpSound;
        public EventReference landSound;
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