using System.Collections.Generic;
using UnityEngine;

public enum EHeardSoundCategory
{
    EFootstep,
    EJump,
    EInteractable
}

public class HearingManager : MonoBehaviour
{
    public static HearingManager Instance { get; private set; } = null;

    public List<SoundDetector> AllSoundDetectors { get; private set; } = new List<SoundDetector>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple HearingManager found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void Register(SoundDetector soundDetector)
    {
        AllSoundDetectors.Add(soundDetector);
    }

    public void Deregister(SoundDetector soundDetector)
    {
        AllSoundDetectors.Remove(soundDetector);
    }

    public void OnSoundEmitted(GameObject source, Vector3 location, EHeardSoundCategory category, float intensity)
    {
        foreach(var soundDetector in AllSoundDetectors)
        {
            soundDetector.OnHeardSound(source, location, category, intensity);
        }
    }
}
