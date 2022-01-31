using UnityEngine;
using UnityEngine.SceneManagement;
 
/// <summary>
/// Controls the pausing system with input
/// music for pause menu here
/// </summary>

public class PauseMenu : InputController<PauseSystem>
{

    private static bool GameIsPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown( controls.pause )) 
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        //disables the pause panel and resumes the speed
        system.pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        //disables cursor 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //if audio wants to be paused not sure yet
        //AudioListener.pause = false;
    }

    void Pause()
    {
        //activates the pause panel
        system.pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        //enables cursor for pause panel
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //if audio wants to be paused not sure yet
        //AudioListener.pause = true;

    }

    // ADD MAIN MENU SCENE HERE
    public void LoadMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

