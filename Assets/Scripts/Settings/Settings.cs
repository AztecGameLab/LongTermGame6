using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class Settings : MonoBehaviour
{
    [Header("Dependencies")]
    
    [SerializeField] 
    private AudioMixer masterAudioMixer;
    
    [SerializeField] 
    private AudioMixer sfxAudioMixer;
    
    [SerializeField] 
    private AudioMixer musicAudioMixer;

    [SerializeField] 
    private TMP_Dropdown resolutionDropdown;
    
    [SerializeField] 
    [Tooltip("Scriptable object for player that will replace the default controls scriptable object")]
    private Controls customControls;

    private Resolution[] _resolutions;

    private void Start()
    {
        _resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        var resolutionOptions = new List<string>();

        int currentResolutionsIndex = 0;

        //for loops fills the fills the resolutions dropdown
        for(int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height + " : " + _resolutions[i].refreshRate;
            resolutionOptions.Add(option);

            if(_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionsIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionsIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetMasterVolume(float volume)
    {
        masterAudioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        sfxAudioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        customControls.sensitivity = sensitivity;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void VsyncToggle()
    {
        QualitySettings.vSyncCount = QualitySettings.vSyncCount > 0 ? 0 : 1;
    }
}
