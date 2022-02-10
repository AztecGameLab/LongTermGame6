using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public AudioMixer masterAudioMixer;
    public AudioMixer SFXAudioMixer;
    public AudioMixer musicAudioMixer;

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public Controls pain;
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionsIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            resolutionOptions.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
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
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetMasterVolume(float volume)
    {
        //Updates the mixer with the slider value
        masterAudioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        //Updates the mixer with the slider value
        SFXAudioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        //Updates the mixer with the slider value
        musicAudioMixer.SetFloat("MusicVolume", volume);
    }

   // public void SetMouseSensitiviy()
   // {
    //    MouseLook
    //}

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void VsyncToggle()
    {
        if (QualitySettings.vSyncCount > 0)
        {
            QualitySettings.vSyncCount = 0;
            Debug.Log("vSyncCount = 0");
        }
        else
        {
            QualitySettings.vSyncCount = 1;
            Debug.Log("vSyncCount = 1");
        }
    }



    
}
