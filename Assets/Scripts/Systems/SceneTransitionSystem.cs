using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionSystem : MonoBehaviour
{
    [Header("Settings")]
    
    [SerializeField]
    private float fadeOutDuration = 3f;
    
    [SerializeField]
    private float fadeInDuration = 3f;
    
    [Header("Dependencies")]
    
    [SerializeField]
    private CanvasGroup fadeCanvas;

    private static SceneTransitionSystem _instance;

    public static SceneTransitionSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                var systemPrefab = Resources.Load<SceneTransitionSystem>("Scene Transition System");
                var systemInstance = Instantiate(systemPrefab);
                DontDestroyOnLoad(systemInstance);

                _instance = systemInstance;
            }
            
            return _instance;
        }
    }
    
    public void TransitionToScene(string sceneName, Action postLoadCallback = null)
    {
        StopAllCoroutines();
        StartCoroutine(LoadSceneWithFade(sceneName, postLoadCallback));
    }
    
    private IEnumerator LoadSceneWithFade(string sceneName, Action postLoadCallback)
    {
        fadeCanvas.interactable = true;
        fadeCanvas.blocksRaycasts = true;
        
        while (fadeCanvas.alpha < 1)
        {
            fadeCanvas.alpha = Mathf.Clamp01(fadeCanvas.alpha + 1 / fadeOutDuration * Time.deltaTime);
            yield return null;
        }
        
        yield return SceneManager.LoadSceneAsync(sceneName);
        postLoadCallback?.Invoke();

        while (fadeCanvas.alpha > 0)
        {
            fadeCanvas.alpha = Mathf.Clamp01(fadeCanvas.alpha - 1 / fadeInDuration * Time.deltaTime);
            yield return null;
        }
        
        fadeCanvas.interactable = false;
        fadeCanvas.blocksRaycasts = false;
    }
}
