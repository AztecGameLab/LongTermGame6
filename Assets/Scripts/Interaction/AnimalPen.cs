using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPen : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] public GameObject keyObject;
    [SerializeField] public GameObject itemToSpawn;
    [SerializeField] public GameObject pig;
    [SerializeField] public GameObject smallPig1;
    [SerializeField] public GameObject smallPig2;
    [SerializeField] private GameObject newKey;
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(keyObject))
        {

            Destroy(pig);
            Destroy(keyObject);
            smallPig1.SetActive(true);
            smallPig2.SetActive(true);
            newKey.SetActive(true);
            Destroy(gameObject); 
        }
    }
}
