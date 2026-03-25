using SpiritBond.Inventory;
using SpiritBond.UI.Core;
using SpiritBond.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace SpiritBond.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class BtnBagUI : MonoBehaviour
    {
        [SerializeField] private PanelBagUI panelBag;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(OnClick);
            }
        }

        public void OnClick()
        {
            Debug.Log("[BtnBagUI] Btn_Bag clicked.");

            InventoryManager.EnsureRuntimeInstance();

            if (panelBag == null)
            {
                panelBag = PanelBagUI.FindOrCreateInScene();
            }

            if (UIManager.Instance != null)
            {
                UIManager.Instance.Toggle(panelBag);
                return;
            }

            panelBag?.Toggle();
        }
    }
}
