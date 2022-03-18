using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

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
    private TMP_Dropdown vSyncDropdown;

    [SerializeField] 
    [Tooltip("Scriptable object for player that will replace the default controls scriptable object")]
    private Controls customControls;

    [Header("Settings")]

    [SerializeField]
    private GameObject masterMixerSlider;

    [SerializeField]
    private GameObject sfxMixerSlider;

    [SerializeField]
    private GameObject musicMixerSlider;

    [SerializeField]
    [Range(0,2)]
    [Tooltip("0 = low, 1 = medium, 2 = high")]
    private int qualitySettingOnStart;

    [SerializeField][Range(0,100)] //TO DO determine what the range is for the sensitivity
    private int mouseSensitivityOnStart;

    [SerializeField]
    [Range(0, 1)]
    [Tooltip("0 = off, 1 = on")]
    private int vSyncSettingOnStart;



    private Resolution[] _resolutions;

    

    private void Start()
    {
        InitializeResolutions();
        SetQuality(qualitySettingOnStart);
        SetMouseSensitivity(mouseSensitivityOnStart);
        InitializeVsync();
        UpdateSliders();
    }

    private void UpdateSliders()
    {
        float temp;
        masterAudioMixer.GetFloat("MasterVolume", out temp);
        masterMixerSlider.GetComponent<Slider>().value = temp;

        sfxAudioMixer.GetFloat("SFXVolume", out temp);
        sfxMixerSlider.GetComponent<Slider>().value = temp;

        musicAudioMixer.GetFloat("MusicVolume", out temp);
        musicMixerSlider.GetComponent<Slider>().value = temp;
    }

    private void InitializeResolutions()
    {
        _resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        var resolutionOptions = new List<string>();

        int currentResolutionsIndex = 0;

        //for loops fills the fills the resolutions dropdown
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height + " : " + _resolutions[i].refreshRate;
            resolutionOptions.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionsIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionsIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void InitializeVsync()
    {
        vSyncDropdown.value = QualitySettings.vSyncCount;
        vSyncDropdown.RefreshShownValue();
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
        QualitySettings.vSyncCount = vSyncDropdown.value;
    }
}
