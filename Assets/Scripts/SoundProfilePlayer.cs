using System;
using System.Collections.Generic;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

namespace Game
{
    public class SoundProfilePlayer : MonoBehaviour
    {
        [SerializeField] private EntitySoundProfile profile;
        [SerializeField] private GroundCheck groundCheck;
        [SerializeField] private CrouchSystem crouchSystem;

        [SerializeField] private List<TerrainLayerDefinition> definitions;
        
        [Serializable]
        private struct TerrainLayerDefinition
        {
            public int terrainLayer;
            
            [Tag]
            public string tag;
        }
        
        private TerrainDetector _detector;

        private void Awake()
        {
            _detector = new TerrainDetector(Terrain.activeTerrain);
        }

        public void PlayStep()
        {
            string surfaceTag = GetTag();
            string path = GetStepPath(surfaceTag);
            
            RuntimeManager.PlayOneShotAttached(path, gameObject);
        }

        private string GetStepPath(string surfaceTag) 
        {
            if (crouchSystem != null && crouchSystem.IsCrouching)
                return profile.FindSurface(surfaceTag).crouchedStepSound;
            
            return profile.FindSurface(surfaceTag).stepSound;
        }

        private string GetTag()
        {
            string result = groundCheck.ConnectedCollider.tag;

            if (groundCheck.ConnectedCollider.TryGetComponent(out Terrain _))
            {
                int terrainLayer = _detector.GetTextureAt(transform.position);
                
                foreach (var layerDefinition in definitions)
                {
                    if (layerDefinition.terrainLayer == terrainLayer)
                        result = layerDefinition.tag;
                }
            }

            return result;
        }

        public void PlayJump()
        {
            string path = profile.FindSurface(groundCheck.ConnectedCollider.tag).jumpSound;
            RuntimeManager.PlayOneShotAttached(path, gameObject);
        }
    }
}