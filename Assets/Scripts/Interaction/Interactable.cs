using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents an object that can be interacted with (pressing a button, grabbing a box, turning a wheel ect).
/// </summary>

public class Interactable : MonoBehaviour
{
    [Header("Settings")]
    
    [SerializeField] 
    [Tooltip("How close must you be to this object to interact with it.")]
    private float interactRange = 1.5f;
    
    [SerializeField] 
    [Tooltip("How often will this object check if its line of sight is interupted from the player. If it is it will drop itself")]
    private float lineOfSightCheckInterval = 1.5f;
    [Space(20f)]
    
    [SerializeField]
    [Tooltip("Passes the object interacting with us, and the position they selected.")]
    public UnityEvent<GameObject, Vector3> onInteractStart;
    
    [SerializeField]
    [Tooltip("Called when this object stops being interacted with.")]
    public UnityEvent onInteractEnd;
    // Internal State
    
    public float InteractRange => interactRange;
    public bool IsHeld { get; private set; }
    public Vector3 GrabPoint { get; private set; }
    private Transform holder;
    private Transform CameraPosition;
    private float lastLineOfSightCheck;
    // Methods
    private void Awake(){
        CameraPosition=Camera.main.transform;
    }
    public void InteractStart(GameObject source, Vector3 point)
    {
        GrabPoint = point;
        IsHeld = true;
        onInteractStart.Invoke(source, point);
        holder=source.transform;
        lastLineOfSightCheck=Time.time;
    }

    public void InteractEnd()
    {
        IsHeld = false;
        onInteractEnd.Invoke();
    }

    private void Update(){
        if(IsHeld&&holder.root.CompareTag("Player")&&lineOfSightCheckInterval<Time.time-lastLineOfSightCheck){
            lastLineOfSightCheck=Time.time;
            RaycastHit hit;
            int layerStorage=gameObject.layer;
            gameObject.layer=2;
            bool isPlayerInLineOfSight=false;
            if(Physics.Raycast(transform.position,Camera.main.transform.position-transform.position,out hit,Mathf.Infinity)){
                if(hit.transform.root.CompareTag("Player")){
                    isPlayerInLineOfSight=true;
                }
            }
            if(!isPlayerInLineOfSight)
                InteractEnd();
            gameObject.layer=layerStorage;
        }
    }
}