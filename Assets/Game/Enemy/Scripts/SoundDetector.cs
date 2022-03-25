using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class SoundDetector : MonoBehaviour
{
    [Header("Settings")] 
    
    [SerializeField] 
    private float hearingRange = 20f;
    
    [SerializeField] 
    private Color hearingRangeColor = new Color(1f, 1f, 0f, 0.25f);
    
    public float HearingRange => hearingRange;
    public Color HearingRangeColor => hearingRangeColor;
    public Vector3 CurrentPosition => transform.position;
    
    public Vector3 HeardPosition { get; private set; }
    public bool HasSound { get; internal set; }
   
    private void Start()
    {
        HasSound = false;
        if (HearingManager.Instance != null)
        {
            HearingManager.Instance.Register(this);
        }
    }

    private void OnDestroy()
    {
        if (HearingManager.Instance != null)
        {
            HearingManager.Instance.Deregister(this);
        }
    }

    public void OnHeardSound(GameObject source, Vector3 location, EHeardSoundCategory category, float intensity)
    {
        // outside of hearing range
        if (Vector3.Distance(location, CurrentPosition) > HearingRange)
        {
            return;
        }
        
        
        HasSound = true;
        HeardPosition = location;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SoundDetector))]
public class SoundDetectorEditor : Editor
{
    public void OnSceneGUI()
    {
        if (!(target is SoundDetector soundRadius)) return;
        
        // draw the hearing range
        Handles.color = soundRadius.HearingRangeColor;
        Handles.DrawSolidDisc(soundRadius.transform.position, Vector3.up, soundRadius.HearingRange);
    }
}
#endif // UNITY_EDITOR
