using UnityEngine;
using UnityEngine.Events;

public class HingeLocker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float lockedMinAngle = -15;
    [SerializeField] public float lockedMaxAngle = 15;
    [SerializeField] public float openMinAngle = -120;
    [SerializeField] public float openMaxAngle = 120;
    [SerializeField] public int lockAmount = 1;

    [Header("Dependencies")]
    [SerializeField] public HingeJoint hinge;

    [Space(20)] 
    [SerializeField] private UnityEvent onUnlock;

    private int _remainingLocks;
    private JointLimits _lockedLimits;
    private JointLimits _openLimits;

    // Start is called before the first frame update
    private void OnEnable()
    {
        var limits = hinge.limits;
        
        _lockedLimits = limits;
        _lockedLimits.min = lockedMinAngle;
        _lockedLimits.max = lockedMaxAngle;

        _openLimits = limits;
        _openLimits.min = openMinAngle;
        _openLimits.max = openMaxAngle;

        LockHinge();
    }

    public void UnlockHinge()
    {
        _remainingLocks--;

        if (_remainingLocks <= 0)
        {
            hinge.limits = _openLimits;
            onUnlock.Invoke();
        }
    }

    public void LockHinge()
    {
        _remainingLocks = lockAmount;        
        hinge.limits = _lockedLimits;
    }
}
