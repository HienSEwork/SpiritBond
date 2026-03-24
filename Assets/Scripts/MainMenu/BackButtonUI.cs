using UnityEngine;

public class BackButtonUI : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (mainPanel != null)
            mainPanel.SetActive(true);
    }
}