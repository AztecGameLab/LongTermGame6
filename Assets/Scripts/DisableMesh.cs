using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisableMesh : MonoBehaviour
{
    public void ToggleMesh()
    {
        if (transform.TryGetComponent(out MeshCollider col))
        {
            col.enabled = !col.enabled;
        }
        
    }
}
