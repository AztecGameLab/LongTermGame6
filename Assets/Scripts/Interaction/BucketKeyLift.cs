using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketKeyLift : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] public GameObject keyObject;
    [SerializeField] public GameObject openedLockPrefab;
    [SerializeField] public GameObject animatedGameObject;


    //Function makes the animation of the bucket, rope and key play
    //Function also destroys the lock and key and spawns in the opened lock
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(keyObject))
        {
            animatedGameObject.GetComponent<Animator>().SetTrigger("LiftBucket");
            Instantiate(openedLockPrefab, transform.position, transform.rotation);
            Destroy(keyObject);
            Destroy(gameObject);
        }
    }
}
