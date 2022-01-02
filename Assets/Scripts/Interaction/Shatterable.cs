using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shatterable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float minBreakSpeed = 2.5f;

    [Header("Dependencies")]
    [SerializeField] public Rigidbody rigidbody;

    [Space(20)]
    [SerializeField] public UnityEvent onBreak;

    private void OnCollisionEnter(Collision collision)
    {
        if (rigidbody.velocity.magnitude > minBreakSpeed)
        {
            onBreak.Invoke();
        }
    }

    public void Break(){
        Destroy(this.gameObject);
    }
}
