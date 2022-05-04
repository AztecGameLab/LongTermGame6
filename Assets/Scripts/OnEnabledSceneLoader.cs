using NaughtyAttributes;
using UnityEngine;

namespace Game
{
    public class OnEnabledSceneLoader : MonoBehaviour
    {
        [SerializeField, Scene] private string targetScene;

        private void OnEnable()
        {
            SceneTransitionSystem.Instance.TransitionToScene(targetScene);
        }
    }
}