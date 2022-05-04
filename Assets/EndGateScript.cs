using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndGateScript : MonoBehaviour
{
    [SerializeField] int locksToUnlock;

    [SerializeField] public UnityEvent OnUnlock;

    private int _locksTriggered;
    public void addUnlock()
    {
        _locksTriggered++;
        if (_locksTriggered >= locksToUnlock)
        {
            OnUnlock.Invoke();
            SceneTransitionSystem.Instance.TransitionToScene("Cutscene2to3");
        }
    }
}
