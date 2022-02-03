using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPen : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] public GameObject keyObject;
    [SerializeField] public GameObject dummyLockPrefab;
    [SerializeField] public GameObject animatedGameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(keyObject))
        {
            animatedGameObject.GetComponent<Animator>().SetTrigger("Unlocked");
            Instantiate(dummyLockPrefab, transform.position, transform.rotation);
            Destroy(keyObject);
            Destroy(gameObject);
            Destroy(animatedGameObject);
        }
    }
}
