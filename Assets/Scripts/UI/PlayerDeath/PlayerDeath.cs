using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathCanvas;
    public static PlayerDeath instance;
    private Animator animator;
    private bool respawned = false;
    public string sceneName = "StreetLevel";
    public void Awake()
    {
        //Debug.Log("AWAKE");
        instance = this;
        animator = deathCanvas.GetComponent<Animator>();
    }
    private void Update()
    {
        if (gameObject.transform.position.y == 100f)
        {
            Debug.Log("IT IS AT 100");
        }
        if (deathCanvas.activeInHierarchy)
        {
            Respawn();
        }
    }

    public void EnableCanvas()
    {
        deathCanvas.SetActive(true);
        Cursor.visible = true;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }


    public void Respawn()
    {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && respawned == false)
            {
             
            respawned = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gameObject.transform.position = RespawnPosition._instance.respawnPosition;
            SceneManager.LoadScene(sceneName);
           // Debug.Log("this is the y " + gameObject.transform.position.y);
            //respawn player
        } 
    }

}
