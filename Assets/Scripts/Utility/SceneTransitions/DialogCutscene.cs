using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class DialogCutscene : MonoBehaviour
{
    [Header("Scriptable")] 
    [SerializeField] private List<CutscenesInfo> cutscenesInfo;

    [Header("Dependencies")]
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private GameObject arrowContinueGameObject;
    [SerializeField] private TextBoxBehavior textBoxBehaviorScript;

    [Header("Text Settings")]
    [SerializeField] [Tooltip("Font size of text")] 
    private float dialogFontSize;
    [SerializeField] [Tooltip("Font of text")] 
    private TMP_FontAsset dialogFont;
    [SerializeField] [Tooltip("Text color")] 
    private Color dialogColor;

    [Header("Animation Settings in Seconds")]
    [SerializeField] [Tooltip("Animation speed for images in seconds")] 
    private float sceneAnimationSpeed;
    [SerializeField] [Tooltip("Time until text box appears again")] 
    private float textBoxRestingTime;
    [SerializeField] [Tooltip("Time until text begins to be typed")] 
    private float textRestingTime;
    [SerializeField] [Tooltip("Speed of text typing")] 
    private float dialogSpeed;
    
    [Header("Scene Transitions")]
    [SerializeField, Scene] [Tooltip("Scene to be transitioned to")]
    private string nextScene;
    
    //inner variables
    private int _sentenceCount = 0;
    private int _sceneCount = 0;
    private readonly Vector3 _finalAnimationPosition = new Vector3(0f, 0f, 0f);

    private void Start()
    {
        //setting up textDisplay
        textDisplay.color = dialogColor;
        textDisplay.font = dialogFont;
        textDisplay.fontSize = dialogFontSize;
        
        StartCoroutine(StartDialogWriter());
    }
    
    private IEnumerator StartDialogWriter()
    {
        yield return new WaitForSeconds(textBoxRestingTime);
        StartCoroutine(DialogWriter());
    }   
    
    private IEnumerator DialogWriter()
    {
        float remainingTime = cutscenesInfo[_sceneCount].Duration;
        
        yield return textBoxBehaviorScript.FadeInOnEnable();
        yield return new WaitForSeconds(textRestingTime);
        remainingTime -= textRestingTime;
        foreach (char letter in cutscenesInfo[_sceneCount].DialogData[_sentenceCount]) 
        {
            textDisplay.text += letter;
            remainingTime -= dialogSpeed;
            yield return new WaitForSeconds(dialogSpeed);
        }

        arrowContinueGameObject.SetActive(true);
        // yield return new WaitUntil(() => Input.GetMouseButton(0));
        yield return new WaitForSeconds(remainingTime);
        NextSentence();
    }

    private void NextSentence()
    {
        arrowContinueGameObject.SetActive(false);
        textDisplay.text = "";
        
        //if we have a scene transition we execute the transition
        if (_sentenceCount +1 >= cutscenesInfo[_sceneCount].DialogData.Count) 
        {
            StartCoroutine(AnimatorWaitDone());
        }
        else 
        {//same scene keep inputting text 
            _sentenceCount++;
            StartCoroutine(DialogWriter());
        }
    }
    
    private IEnumerator AnimatorWaitDone()
    {
        _sceneCount++;
        if (_sceneCount >= cutscenesInfo.Count)
        {
            NextScene();
            yield break;
        }
        yield return textBoxBehaviorScript.FadeOutOnDisable();
        if ( !cutscenesInfo[_sceneCount].IsFirstScene )
        {//perform image animation
            Debug.Log("executed animation scene");
            float currentTime = 0f;
            Vector3 startPosition = cutscenesInfo[_sceneCount].ImageRectTransform.anchoredPosition;
            while (currentTime <= sceneAnimationSpeed)
            {
                currentTime += Time.deltaTime;
                float normalizedTime = currentTime / sceneAnimationSpeed;

                cutscenesInfo[_sceneCount].ImageRectTransform.anchoredPosition = Vector3.Lerp(startPosition,
                    _finalAnimationPosition, normalizedTime);
                yield return null;
            }
        }

        yield return new WaitForSeconds(textBoxRestingTime);


        _sentenceCount = -1;
        NextSentence();
        yield return null;
    }

    public void NextScene()
    {
        textBoxBehaviorScript.gameObject.SetActive(false);
        arrowContinueGameObject.SetActive(false);
        textDisplay.text = "";
        SceneTransitionSystem.Instance.TransitionToScene(nextScene);
    }
    
}
