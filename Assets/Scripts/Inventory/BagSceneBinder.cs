using SpiritBond.UI.Buttons;
using SpiritBond.UI.Panels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpiritBond.Inventory
{
    public static class BagSceneBinder
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitialize()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!scene.IsValid())
            {
                return;
            }

            InventoryManager.EnsureRuntimeInstance();

            GameObject bagButtonObject = GameObject.Find("Btn_Bag");
            if (bagButtonObject != null && bagButtonObject.GetComponent<Button>() != null && bagButtonObject.GetComponent<BtnBagUI>() == null)
            {
                bagButtonObject.AddComponent<BtnBagUI>();
                Debug.Log("[BagSceneBinder] Added BtnBagUI to Btn_Bag at runtime.");
            }

            GameObject panelObject = GameObject.Find("Panel_Bag");
            if (panelObject != null && panelObject.GetComponent<PanelBagUI>() == null)
            {
                panelObject.AddComponent<PanelBagUI>();
                Debug.Log("[BagSceneBinder] Added PanelBagUI to existing Panel_Bag.");
            }
        }
    }
}
