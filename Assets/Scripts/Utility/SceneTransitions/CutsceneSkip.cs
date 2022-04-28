using UnityEngine;
using UnityEngine.UI;
 
public class CutsceneSkip : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private Image fillImage;
    [SerializeField] [Tooltip("The gameObject that holds both images for skipping")]
    private RectTransform holderRect;
    [SerializeField] 
    private DialogCutscene dialogScript;

    [Header("Settings")]
    [SerializeField] [Tooltip("Scale to be changed to after holding")]
    private Vector2 targetHolderScale;
    [SerializeField] [Tooltip("Scale to be changed to after holding")]
    private KeyCode keyCode;
    [SerializeField] [Tooltip("Speed of progress when pressed")]
    private float activeSpeed;
    [SerializeField] [Tooltip("Speed of progress decreased when not pressed")]
    private float cooldownSpeed;
    
    //inner 
    private bool _functionTriggered = false;
    private Vector2 _startHolderScale;
 
    private void Awake()
    {
        _startHolderScale = holderRect.localScale;
        fillImage.fillAmount=0;
    }

    private void Update()
    {
        SkipCutscene();
    }

    private void SkipCutscene()
    {
        fillImage.fillAmount += 
            ((Input.GetKey(keyCode) && !_functionTriggered) ? activeSpeed : -cooldownSpeed) * Time.deltaTime;
         
        holderRect.localScale = Vector2.Lerp(
            _startHolderScale,
            targetHolderScale,
            fillImage.fillAmount * fillImage.fillAmount
        );
        
        if (!_functionTriggered && fillImage.fillAmount == 1)
        {
            _functionTriggered = true;
            dialogScript.NextScene();
        }
    }
}
