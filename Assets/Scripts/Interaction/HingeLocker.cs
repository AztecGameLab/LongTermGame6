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

    JointLimits lockedLimits;
    JointLimits openLimits;

    [Header("Dependencies")]
    [SerializeField] public HingeJoint hinge;


    // Start is called before the first frame update
    private void OnEnable()
    {
        lockedLimits = hinge.limits;
        lockedLimits.min = lockedMinAngle;
        lockedLimits.max = lockedMaxAngle;

        openLimits = hinge.limits;
        openLimits.min = openMinAngle;
        openLimits.max = openMaxAngle;

        hinge.limits = lockedLimits;
    }

    public void unlockHinge()
    {
        hinge.limits = openLimits;
    }

    public void lockHinge(){
        hinge.limits = lockedLimits;
    }
}
