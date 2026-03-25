using SpiritBond.UI.Core;
using SpiritBond.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace SpiritBond.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class CloseButtonUI : MonoBehaviour
    {
        [SerializeField] private PanelPetUI panelPet; // Assign in Inspector

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
            Debug.Log("[CloseButtonUI] Btn_Close clicked.");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.Hide(panelPet);
                return;
            }

            Debug.LogWarning("[CloseButtonUI] UIManager not found. Using direct panel hide.");
            panelPet?.Hide();
        }
    }
}
