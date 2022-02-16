using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Player Prefab has a child named 'Controllers'")]
    [SerializeField] public int playerControllerChildIndex;
    private Resolution[] _resolutions;

    [Header("Dependencies")]
    [SerializeField] private AudioMixer masterAudioMixer;
    [SerializeField] private AudioMixer SFXAudioMixer;
    [SerializeField] private AudioMixer musicAudioMixer;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [Tooltip("Scriptable object for player that will replace the default controls scriptable object")]
    [SerializeField] private Controls customControls;
    [SerializeField] private GameObject Player;

    

    private void Start()
    {
        _resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionsIndex = 0;

        //for loops fills the fills the resolutions dropdown
        for(int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
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

    public void SetSFXVolume(float volume)
    {
        SFXAudioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetMouseSensitiviy(float sensitivity)
    {
        SwitchToCustomControls();
        customControls.sensitivity = sensitivity;
    }

    void SwitchToCustomControls()
    {
        if(Player.transform.GetChild(playerControllerChildIndex).GetComponent<InputRotationController>().controls != customControls)
        {
            Player.transform.GetChild(playerControllerChildIndex).GetComponent<InputRotationController>().controls = customControls;
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void VsyncToggle()
    {
        if (QualitySettings.vSyncCount > 0)
        {
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            QualitySettings.vSyncCount = 1;
        }
    }



    
}
