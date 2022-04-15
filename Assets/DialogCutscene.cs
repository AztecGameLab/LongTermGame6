using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class DialogCutscene : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private GameObject arrowContinueGameObject;
    [SerializeField] private TextBoxBehavior textBoxBehaviorScript;
    [SerializeField] private RectTransform[] cutscenesRectTransforms; 
    [SerializeField] private string[] sentences;

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
    private int _indexSentence = 0;
    private int _indexObject = 0;
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
        
        yield return textBoxBehaviorScript.FadeInOnEnable();
        yield return new WaitForSeconds(textRestingTime);
        foreach (char letter in sentences[_indexSentence])
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(dialogSpeed);
        }

        arrowContinueGameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        NextSentence();
    }

    private void NextSentence()
    {
        //if we have a scene transition we execute the transition
        if (_indexSentence + 1 < sentences.Length && sentences[_indexSentence + 1] == "-----CUTSCENE-----" )
        {
            //yield return textBoxBehaviorScript.FadeOutOnDisable();
            arrowContinueGameObject.SetActive(false);
            textDisplay.text = "";
            if (_indexSentence < sentences.Length - 1)
            {
                StartCoroutine(AnimatorWaitDone());
            }
            else
            {
                NextScene();
            }
        }
        else 
        {//same scene keep inputting text 
            arrowContinueGameObject.SetActive(false);
            textDisplay.text = "";
            if (_indexSentence < sentences.Length - 1)
            {
                _indexSentence++;
                StartCoroutine(DialogWriter());
            }
            else
            {
                NextScene();
            }
        }
    }
    
    private IEnumerator AnimatorWaitDone()
    {
        yield return textBoxBehaviorScript.FadeOutOnDisable();
        if (_indexObject < cutscenesRectTransforms.Length )
        {//perform image animation
            float currentTime = 0f;
            Vector3 startPosition = cutscenesRectTransforms[_indexObject].anchoredPosition;
            while (currentTime <= sceneAnimationSpeed)
            {
                currentTime += Time.deltaTime;
                float normalizedTime = currentTime / sceneAnimationSpeed;

                cutscenesRectTransforms[_indexObject].anchoredPosition = Vector3.Lerp(startPosition,
                    _finalAnimationPosition, normalizedTime);
                yield return null;
            }
            _indexObject++;
        }

        yield return new WaitForSeconds(textBoxRestingTime);
        
        if (_indexSentence < sentences.Length - 1)
        {
            _indexSentence++;
            NextSentence();
        }
    }

    public void NextScene()
    {
        textBoxBehaviorScript.gameObject.SetActive(false);
        arrowContinueGameObject.SetActive(false);
        textDisplay.text = "";
        SceneTransitionSystem.Instance.TransitionToScene(nextScene);
    }
    
}
