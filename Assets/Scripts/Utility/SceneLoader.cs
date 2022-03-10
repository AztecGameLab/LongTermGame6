using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class SceneLoader : MonoBehaviour
    {
        public void Load(string sceneName)
        {
            SceneTransitionSystem.Instance.TransitionToScene(sceneName);
        }

        public void Reload()
        {
            Load(SceneManager.GetActiveScene().name);
        }
    }
}