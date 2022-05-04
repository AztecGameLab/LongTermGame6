using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField]
    float vel;

    [SerializeField]
    AnimationCurve ac;

    [SerializeField]
    RectTransform rt;

    [SerializeField]
    CanvasGroup img;

    float _start;


    private IEnumerator Start()
    {
        _start = Time.time;
        yield return new WaitForSeconds(91);
        SceneTransitionSystem.Instance.TransitionToScene("MainMenu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneTransitionSystem.Instance.TransitionToScene("MainMenu");
        }
        rt.position += new Vector3(0, vel * Time.deltaTime, 0);
        img.alpha = ac.Evaluate(Time.time- _start);
    }
}
