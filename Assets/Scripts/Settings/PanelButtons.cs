using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//The purpose of the script is to bring the panel that is selected in front of the other panels
public class PanelButtons : MonoBehaviour
{
    
    [Header("Dependencies")]
    public GameObject controlsPanel;
    public GameObject audioPanel;
    public GameObject graphicsPanel;
    public GameObject settingsCanvas;

    public void BringControlPanelForward()
    {
        controlsPanel.transform.SetSiblingIndex(settingsCanvas.transform.childCount - 2);
    }
    public void BringGraphicsPanelForward()
    {
        graphicsPanel.transform.SetSiblingIndex(settingsCanvas.transform.childCount - 2);
    }
    public void BringAudioPanelForward()
    {
        audioPanel.transform.SetSiblingIndex(settingsCanvas.transform.childCount - 2);
    }
}
