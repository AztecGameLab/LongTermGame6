using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelButtons : MonoBehaviour
{
    public GameObject controlsPanel;
    public GameObject audioPanel;
    public GameObject graphicsPanel;

    public GameObject gameobjectClosestToScreen;
    public GameObject settingsCanvas;

    public void BringControlPanelForward()
    {
        controlsPanel.transform.SetSiblingIndex(settingsCanvas.transform.childCount - 2); //setttings canvas child count minus one
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
