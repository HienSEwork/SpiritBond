using UnityEngine;

public class SettingsButtonUI : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;

    public void OpenSettings()
    {
        if (mainPanel != null)
            mainPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }
}