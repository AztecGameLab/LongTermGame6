using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : MonoBehaviour
{
    public GameObject firstEnemy;

    void Start()
    {
        firstEnemy = GameObject.Find("TutorialEnemy");
        firstEnemy.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !firstEnemy.activeInHierarchy)
        {
            Debug.Log("Trigger Entered");
            firstEnemy.SetActive(true);
        }
    }
}
