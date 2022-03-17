using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("DEPENDENCIES")]
    [SerializeField] private GameObject keyObject;   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(keyObject))
        {
            //Save.WriteData();
        }
    }


}
