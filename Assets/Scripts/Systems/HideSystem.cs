using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class HideSystem : MonoBehaviour
{
    //TODO: weird bug with the pause menu
    //TODO: AI detection disable

    [Header("Settings")]
    [SerializeField]
    public float hideTime;
    [SerializeField]
    public float unhideTime;

    [Header("Dependencies")]
    [SerializeField]
    public Transform playerTransform;
    [SerializeField]
    public Transform camTargetTransform;

    [SerializeField]
    public UnityEvent OnHide;
    [SerializeField]
    public UnityEvent OnUnHide;

    private bool _isHideingAnimation = false;
    private bool _isUnHideingAnimation = false;

    private float _hideStartTime;

    private Transform _endTransform;

    public void hide(Transform start, Transform transition, Transform end)
    {
        _hideStartTime = Time.time;

        _isHideingAnimation = true;
        _isUnHideingAnimation = false;

        _endTransform = end;

        OnHide.Invoke();
    }

    public void unhide(Transform end)
    {
        _hideStartTime = Time.time;

        _isHideingAnimation = false;
        _isUnHideingAnimation = true;

        _endTransform = end;

        OnUnHide.Invoke();
    }

    private void Update()
    {
        if (_isHideingAnimation)
        {
            ApplyHideAnimation(_endTransform.position);
        }
        if (_isUnHideingAnimation)
        {
            ApplyUnHideAnimation(_endTransform.position);
        }
    }
    //credit to Daniel Walls (@poe) for framework
    private void ApplyHideAnimation(Vector3 endPos)
    {
        float elapsedTime = Time.time - _hideStartTime;
        float percentDone = math.smoothstep(0, hideTime, elapsedTime * Time.timeScale);

        playerTransform.position = Vector3.Lerp(camTargetTransform.position, endPos, percentDone);

    }

    private void ApplyUnHideAnimation(Vector3 endPos)
    {
        float elapsedTime = Time.time - _hideStartTime;
        float percentDone = math.smoothstep(0, unhideTime, elapsedTime * Time.timeScale);

        playerTransform.position = Vector3.Lerp(endPos, camTargetTransform.position, percentDone);

        if (percentDone > 1)
        {
            _isHideingAnimation = false;
            _isUnHideingAnimation = false;
        }

    }
}
