using UnityEngine;

//The purpose of the script is to bring the panel that is selected in front of the other panels
public class PanelButtons : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject graphicsPanel;
    [SerializeField] private GameObject settingsCanvas;

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
