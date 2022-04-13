using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogCutscene : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private GameObject arrowContinueGameObject;
    [SerializeField] private GameObject textboxGameObject;
    [SerializeField] private Animator[] cutscenesAnimator; 
    [SerializeField] private string[] sentences;

    [Header("Settings")]
    [SerializeField] private float dialogFontSize;
    [SerializeField] private TMP_FontAsset dialogFont;
    [SerializeField] private Color dialogColor;
    [SerializeField] private float dialogSpeed;

    private int _indexSentence = 0;
    private int _indexObject = 0;
    
    private void Start()
    {
        //setting up textDisplay
        textDisplay.color = dialogColor;
        textDisplay.font = dialogFont;
        textDisplay.fontSize = dialogFontSize;
        
        StartCoroutine(DialogWritter());
    }
    
    IEnumerator DialogWritter()
    {
        
        textboxGameObject.SetActive(true);
        foreach (char letter in sentences[_indexSentence])
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(dialogSpeed);
        }

        arrowContinueGameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        NextSentence();
    }

    public void NextSentence()
    {
        textboxGameObject.SetActive(false);
        arrowContinueGameObject.SetActive(false);
        textDisplay.text = "";
        if (_indexSentence < sentences.Length - 1)
        {
            StartCoroutine(AnimatorWaitDone());
        }
        else
        {
            SceneManager.LoadScene(0);
            Debug.Log("Ended transitioning next scene");
        }
    }
    
    IEnumerator AnimatorWaitDone()
    {
        if (_indexObject < cutscenesAnimator.Length )
        {
            cutscenesAnimator[_indexObject].SetTrigger("Activate");
            yield return new WaitUntil(() => cutscenesAnimator[_indexObject].GetCurrentAnimatorStateInfo(0).normalizedTime  > 1 );
            _indexObject++;
            Debug.Log("true");
        }

        yield return new WaitForSeconds(3f);
        
        if (_indexSentence < sentences.Length - 1)
        {
            _indexSentence++;
            StartCoroutine(DialogWritter());
        }
    }
    
}
