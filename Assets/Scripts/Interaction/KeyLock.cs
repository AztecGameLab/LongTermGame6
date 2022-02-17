using UnityEngine;
using UnityEngine.Events;

public class KeyLock : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float lockedMinAngle = -15;
    [SerializeField] public float lockedMaxAngle = 15;
    [SerializeField] public float openMinAngle = -120;
    [SerializeField] public float openMaxAngle = 120;

    JointLimits lockedLimits;
    JointLimits openLimits;

    [Header("Dependencies")]
    [SerializeField] public GameObject keyObject;
    [SerializeField] public HingeJoint hinge;
    [Space(10)]
    [SerializeField] public GameObject dummyLockPrefab;

    [Space(20)] 
    [SerializeField] public UnityEvent onUnlock;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(keyObject))
        {
            onUnlock.Invoke();
            
            hinge.limits = openLimits;
            Instantiate(dummyLockPrefab, transform.position, transform.rotation);
            Destroy(keyObject);
            Destroy(gameObject);
        }
    }
}
