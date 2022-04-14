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
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private Transform[] cutscenesTransform; 
    [SerializeField] private string[] sentences;

    [Header("Text Settings")]
    [SerializeField] [Tooltip("Font size of text")] 
    private float dialogFontSize;
    [SerializeField] [Tooltip("Font of text")] 
    private TMP_FontAsset dialogFont;
    [SerializeField] [Tooltip("Text color")] 
    private Color dialogColor;

    [Header("Animation Settings")]
    [SerializeField] [Tooltip("Speed of scene transitions")] 
    private float speedTransformAnimation;
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
    private  Vector3 _finalAnimationPosition;

    private void Start()
    {
        //setting up textDisplay
        textDisplay.color = dialogColor;
        textDisplay.font = dialogFont;
        textDisplay.fontSize = dialogFontSize;
        
        //canvas setup
        _finalAnimationPosition = canvasTransform.position;
        
        StartCoroutine(StartDialogWriter());
    }
    
    private IEnumerator StartDialogWriter()
    {
        yield return new WaitForSeconds(textBoxRestingTime);
        StartCoroutine(DialogWriter());
    }
    
    private IEnumerator DialogWriter()
    {
        
        textBoxBehaviorScript.FadeInOnEnable();
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
            textBoxBehaviorScript.FadeOutOnDisable();
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
        if (_indexObject < cutscenesTransform.Length )
        {
            while (cutscenesTransform[_indexObject].position != _finalAnimationPosition)
            {
                cutscenesTransform[_indexObject].position = Vector3.MoveTowards(cutscenesTransform[_indexObject].position,
                    _finalAnimationPosition, speedTransformAnimation);
                yield return new WaitForEndOfFrame();
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

    private void NextScene()
    {
        textBoxBehaviorScript.gameObject.SetActive(false);
        SceneTransitionSystem.Instance.TransitionToScene(nextScene);
    }
    
}
