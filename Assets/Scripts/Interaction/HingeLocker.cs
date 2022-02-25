using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeLocker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float lockedMinAngle = -15;
    [SerializeField] public float lockedMaxAngle = 15;
    [SerializeField] public float openMinAngle = -120;
    [SerializeField] public float openMaxAngle = 120;

    [Header("Dependencies")]
    [SerializeField] public HingeJoint hinge;


    private JointLimits _lockedLimits;
    private JointLimits _openLimits;

    // Start is called before the first frame update
    private void OnEnable()
    {
        _lockedLimits = hinge.limits;
        _lockedLimits.min = lockedMinAngle;
        _lockedLimits.max = lockedMaxAngle;

        _openLimits = hinge.limits;
        _openLimits.min = openMinAngle;
        _openLimits.max = openMaxAngle;

        hinge.limits = _lockedLimits;
    }

    public void UnlockHinge()
    {
        hinge.limits = _openLimits;
    }

    public void LockHinge(){
        hinge.limits = _lockedLimits;
    }
}
