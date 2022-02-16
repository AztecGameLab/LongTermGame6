using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HideSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    public float hideTime;
    [Header("Dependencies")]
    [SerializeField]
    public Transform playerTransform;

    private bool _isHideingAnimation = false;
    private bool _isUnHideingAnimation = false;

    private float _hideStartTime;

    private Transform _targetTransform;

    public void hide(Transform start, Transform transition, Transform end)
    {
        _isHideingAnimation = true;
        _targetTransform = end;
        _hideStartTime = Time.time;
    }

    public void unhide(Transform end, Transform start)
    {

    }

    private void Update()
    {
        if (_isHideingAnimation)
        {
            ApplyAnimation(_targetTransform.position);
        }
    }

    //credit to Daniel Walls (@poe)
    private void ApplyAnimation(Vector3 targetPosition)
    {
        float elapsedTime = Time.time - _hideStartTime;
        float percentDone = math.smoothstep(0, hideTime, elapsedTime * Time.timeScale);

        playerTransform.position = Vector3.Lerp(playerTransform.position, targetPosition, percentDone);
    }
}
