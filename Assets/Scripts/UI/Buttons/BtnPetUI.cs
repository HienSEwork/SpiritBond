using SpiritBond.UI.Core;
using SpiritBond.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace SpiritBond.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class BtnPetUI : MonoBehaviour
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
            Debug.Log("[BtnPetUI] Btn_Pet clicked.");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.Toggle(panelPet);
                return;
            }

            Debug.LogWarning("[BtnPetUI] UIManager not found. Using direct panel toggle.");
            panelPet?.Toggle();
        }
    }
}
