using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Game
{
    public class SoundProfilePlayer : MonoBehaviour
    {
        [SerializeField] private EntitySoundProfile profile;
        [SerializeField] private GroundCheck groundCheck;
        [SerializeField] private CrouchSystem crouchSystem;
        [SerializeField] private Rigidbody targetRigidbody;

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
            if (Terrain.activeTerrain != null)
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
            string surfaceTag = GetTag();
            string path = profile.FindSurface(surfaceTag).jumpSound;
            RuntimeManager.PlayOneShotAttached(path, gameObject);
        }

        private Queue<float> _groundSpeeds = new Queue<float>();
        
        private void FixedUpdate()
        {
            _groundSpeeds.Enqueue(targetRigidbody.velocity.y);

            if (_groundSpeeds.Count > 3)
                _groundSpeeds.Dequeue();
        }

        [SerializeField] private float maxVelocity = 1;

        public void PlayLand()
        {
            string surfaceTag = GetTag();
            string path = profile.FindSurface(surfaceTag).landSound;
            
            if (targetRigidbody != null)
            {
                var instance = RuntimeManager.CreateInstance(path);
                float t = Mathf.Clamp01(Mathf.Abs(_groundSpeeds.Peek()) / maxVelocity);
                if (t <= 0.1f)
                    t = 0;
                float volume = Mathf.Lerp(0, 1, t);
                instance.setVolume(volume);
                instance.set3DAttributes(new ATTRIBUTES_3D
                {
                    forward = new VECTOR{x = transform.forward.x, y = transform.forward.y, z = transform.forward.z},
                    position = new VECTOR{x = transform.position.x, y = transform.position.y, z = transform.position.z},
                    up = new VECTOR{x = transform.up.x, y = transform.up.y, z = transform.up.z},
                    velocity = new VECTOR{x = targetRigidbody.velocity.x, y = targetRigidbody.velocity.y, z = targetRigidbody.velocity.z},
                });
                instance.start();
                StartCoroutine(DisposeLand(instance));
            }

            else
            {
                RuntimeManager.PlayOneShotAttached(path, gameObject);
            }
        }

        private static IEnumerator DisposeLand(EventInstance instance)
        {
            yield return new WaitForSeconds(1f);
            instance.stop(STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
    }
}