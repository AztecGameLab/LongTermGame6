using UnityEngine;
using UnityEngine.Events;

public class KeyLock : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] public GameObject keyObject;
    [SerializeField] public GameObject dummyLockPrefab;
    [SerializeField] public UnityEvent onUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(keyObject))
        {
            onUnlock.Invoke();
        }
    }

    public void dropLock()
    {
        Instantiate(dummyLockPrefab, transform.position, transform.rotation);
        Destroy(keyObject);
        Destroy(gameObject);
    }

    public void deleteKey()
    {
        Destroy(keyObject);
    }
}
