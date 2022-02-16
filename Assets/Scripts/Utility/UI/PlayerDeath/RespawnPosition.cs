using UnityEngine;

// todo: integrate save system, instead of fixed respawn points

public class RespawnPosition : MonoBehaviour
{
    public static RespawnPosition Instance;
    
    public Vector3 respawnPosition;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
