using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [Header("Settings")]
    
    [Scene]
    [SerializeField]
    private string sceneName = "StreetLevel";
    
    [Header("Dependencies")]
    
    [SerializeField]
    private GameObject deathCanvas;
    
    [SerializeField]
    private Animator animator;

    public static PlayerDeath Instance;
    private bool _respawned;

    public void Awake()
    {
        Instance = this;
        animator = deathCanvas.GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (deathCanvas.activeInHierarchy)
            Respawn();
    }

    public void EnableCanvas()
    {
        deathCanvas.SetActive(true);
        Cursor.visible = true;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }

    public void Respawn()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _respawned == false)
        {  
            _respawned = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gameObject.transform.position = RespawnPosition.Instance.respawnPosition;
            SceneManager.LoadScene(sceneName);
        } 
    }
}
