using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the pausing system with input
/// music for pause menu here
/// </summary>

public class InputPauseController : InputController<GameObject>
{
    [SerializeField, Scene] private string targetScene;
    [SerializeField] private StudioEventEmitter pauseSnapshot;
    
    [Space(20f)]
    [SerializeField] private UnityEvent onPause;
    [SerializeField] private UnityEvent onResume;
    [SerializeField] private GameObject[] objects;
    
    private static bool _gameIsPaused;

    private void Update()
    {
        if (Input.GetKeyDown(controls.pause)) 
        {
            if (_gameIsPaused)
            {
                Resume();
                ToggleObjects(false);
            }
            else {
                ToggleObjects(true);
                Pause();
            }
        }
    }

    public void Resume()
    {
        // resumes the speed
        system.SetActive(false);
        Time.timeScale = 1f;
        _gameIsPaused = false;
        
        // pauseSnapshot.SetParameter("Intensity", 0);
        onResume.Invoke();
    }

    public void Pause()
    {
        // stop time
        system.SetActive(true);
        Time.timeScale = 0f;
        _gameIsPaused = true;
        
        // pauseSnapshot.SetParameter("Intensity", 100);
        onPause.Invoke();
    }

    public void LoadMenu()
    {
        Resume();
        SceneManager.LoadScene(targetScene);
    }

    private void ToggleObjects(bool hide)
    {
        foreach(GameObject obj in objects)
        {
            obj.SetActive(!hide);
        }
    }
}