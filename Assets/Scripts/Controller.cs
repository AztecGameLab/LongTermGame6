using UnityEngine;

public abstract class Controller<T> : MonoBehaviour where T : MyNamespace.System
{
    [Header("Dependencies")] 
    [SerializeField] protected T system;
    
    public bool IsRunning { get; set; } = true;
    
    private void Start()
    {
        system.Initialize();
    }
}