using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneTransitionSystem : MonoBehaviour
{
    [Header("settings")]
    [SerializeField]
    private float fadeOutDuration;
    [SerializeField]
    private float fadeInDuration;
    [SerializeField]
    private bool developmentMode;
    [Header("dependencies")]
    [SerializeField]
    private Image fadeCover;
    [Header("internal")]
    public static SceneTransitionSystem instance;
    

    private void Awake()
    {
        if (instance)
        {
            if (this != instance)
                Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
    public void TransitionToScene(int buildIndex) 
    {
        StopAllCoroutines();
        StartCoroutine(LoadSceneWithFade(buildIndex));
    }
    private IEnumerator LoadSceneWithFade(int buildIndex)
    { 
        while (fadeCover.color.a<1)
        {
            fadeCover.color=new Color(0,0,0,Mathf.Clamp01( fadeCover.color.a +1/fadeOutDuration*Time.deltaTime));
            yield return null;
            
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        while (fadeCover.color.a > 0)
        {
            fadeCover.color = new Color(0, 0, 0, Mathf.Clamp01(fadeCover.color.a - 1/fadeOutDuration * Time.deltaTime));
            yield return null;

        }
    }
    //Left Shift and a number key to go to that scene in development mode
    private void Update()
    {
        if (developmentMode && Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                print("Loading Scene 0");
                TransitionToScene(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                print("Loading Scene 1");
                TransitionToScene(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                print("Loading Scene 2");
                TransitionToScene(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                print("Loading Scene 3");
                TransitionToScene(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                print("Loading Scene 4");
                TransitionToScene(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                print("Loading Scene 5");
                TransitionToScene(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                print("Loading Scene 6");
                TransitionToScene(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                print("Loading Scene 7");
                TransitionToScene(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                print("Loading Scene 8");
                TransitionToScene(8);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                print("Loading Scene 9");
                TransitionToScene(9);
            }
        }
    }

}
