using UnityEngine;

// todo: fill out remaining settings
// todo: hook up into settings menu

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [Header("Audio")]
    public float overallVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    [Header("Video")]
    public bool fullscreen = true;
    public bool vSync = true;
    public int targetFrameRate = 144;

    // todo: resolution
    // todo: brightness
    // todo wishlist: color blind settings

    [Header("Gameplay")] 
    public float fov = 75f;
    // todo wishlist: subtitles
    // todo wishlist: language

}