using UnityEngine;

namespace UI
{
    public class QuitGameBehaviour : MonoBehaviour
    {
        // todo: add option to save game before quitting
        
        public void Run()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}