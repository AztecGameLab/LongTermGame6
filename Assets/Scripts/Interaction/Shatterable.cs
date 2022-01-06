using UnityEngine;
using UnityEngine.Events;

public class Shatterable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float minBreakSpeed = 2.5f;

    [Header("Dependencies")]
    [SerializeField] public Rigidbody targetRigidbody;

    [Space(20)]
    [SerializeField] public UnityEvent onBreak;

    private void OnCollisionEnter()
    {
        if (targetRigidbody.velocity.magnitude > minBreakSpeed)
            onBreak.Invoke();
    }

    public void Break()
    {
        Destroy(gameObject);
    }
}
