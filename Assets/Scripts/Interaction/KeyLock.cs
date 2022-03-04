using UnityEngine;
using UnityEngine.Events;

public class KeyLock : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] public GameObject keyObject;
    [SerializeField] public GameObject dummyLockPrefab;
    [SerializeField] public UnityEvent OnUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(keyObject))
        {
            OnUnlock.Invoke();
        }
    }

    public void DropLock()
    {
        Instantiate(dummyLockPrefab, transform.position, transform.rotation);
        Destroy(keyObject);
        Destroy(gameObject);
    }

    public void DeleteKey()
    {
        Destroy(keyObject);
    }
}
