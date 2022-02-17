using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaybaleEventHandler : MonoBehaviour
{
    [SerializeField]
    HideSystem hideSystem;

    [SerializeField]
    Transform start;

    [SerializeField]
    Transform transition;

    [SerializeField]
    Transform end;

    private bool _isHideing;

    private void Awake()
    {
        _isHideing = false;
    }

    public void hidePlayer()
    {
        if (!_isHideing)
        {
            hideSystem.hide(start, transition, end);
            _isHideing = true;
        } else
        {
            hideSystem.unhide(end);
            _isHideing = false;
        }
    }
}
