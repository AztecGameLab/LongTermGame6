using System.Linq;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    [SerializeField] [Tooltip("Tags to be ignored on collision")]
    private string[] tagsIgnored;

    [SerializeField] [Tooltip("The minimum velocity the object needs to emit sound")]
    private float velocityThreshold;
    
    private bool _isInstanceNotNull;

    private void Start()
    {
        _isInstanceNotNull = HearingManager.Instance != null;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (tagsIgnored.Contains(other.gameObject.tag)) return;//ignored collisions
        
        //report collision sound if crashed against another collider with certain velocity threshold
        if (_isInstanceNotNull && other.relativeVelocity.magnitude > velocityThreshold)
        {
            HearingManager.Instance.OnSoundEmitted(gameObject, transform.position, EHeardSoundCategory.EInteractable, .5f);
        }
    }
}
