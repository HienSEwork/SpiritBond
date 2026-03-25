using SpiritBond.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace SpiritBond.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class ItemSlotUI : MonoBehaviour
    {
        [SerializeField] private Text itemNameText;
        [SerializeField] private Text amountText;
        [SerializeField] private Image iconImage;

        private InventoryItemStack currentStack;
        private Button button;
        private System.Action<InventoryItemStack> onClick;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void BindRuntimeReferences(Text nameLabel, Text amountLabel, Image icon)
        {
            itemNameText = nameLabel;
            amountText = amountLabel;
            iconImage = icon;
        }

        public void Setup(InventoryItemStack itemStack, System.Action<InventoryItemStack> onClickCallback)
        {
            currentStack = itemStack;
            onClick = onClickCallback;

            if (itemNameText != null)
            {
                itemNameText.text = itemStack?.itemData != null ? itemStack.itemData.itemName : "-";
            }

            if (amountText != null)
            {
                amountText.text = itemStack != null ? $"x{itemStack.amount}" : "x0";
            }

            if (iconImage != null)
            {
                iconImage.sprite = itemStack?.itemData != null ? itemStack.itemData.icon : null;
                iconImage.enabled = iconImage.sprite != null;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            onClick?.Invoke(currentStack);
        }
    }
}
