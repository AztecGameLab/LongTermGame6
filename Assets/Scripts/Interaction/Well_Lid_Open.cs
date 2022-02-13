using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well_Lid_Open : MonoBehaviour
{
    [Header("Dependency")]
    [SerializeField] GameObject keyObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(keyObject))
        {
            Debug.Log("Collision");
            Destroy(this.gameObject);
        }
    }
}
