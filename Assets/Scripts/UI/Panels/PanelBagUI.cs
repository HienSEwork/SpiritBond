using System.Collections.Generic;
using SpiritBond.Inventory;
using SpiritBond.UI.Buttons;
using SpiritBond.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpiritBond.UI.Panels
{
    public class PanelBagUI : UIPanel
    {
        [Header("List")]
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private Button itemSlotTemplate;

        [Header("Detail")]
        [SerializeField] private Text itemNameText;
        [SerializeField] private Text itemAmountText;
        [SerializeField] private Text itemDescriptionText;
        [SerializeField] private Button useItemButton;

        private readonly List<GameObject> spawnedSlots = new List<GameObject>();
        private InventoryItemStack selectedItem;
        private InventoryManager inventoryManager;

        public static PanelBagUI FindOrCreateInScene()
        {
            GameObject panelObject = GameObject.Find("Panel_Bag");
            if (panelObject == null)
            {
                panelObject = CreateRuntimePanelObject();
            }

            PanelBagUI panel = panelObject.GetComponent<PanelBagUI>();
            if (panel == null)
            {
                panel = panelObject.AddComponent<PanelBagUI>();
            }

            panel.EnsureRuntimeLayout();
            return panel;
        }

        private void Awake()
        {
            EnsureRuntimeLayout();
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            inventoryManager = InventoryManager.EnsureRuntimeInstance();
            inventoryManager.InventoryChanged -= HandleInventoryChanged;
            inventoryManager.InventoryChanged += HandleInventoryChanged;

            if (useItemButton != null)
            {
                useItemButton.onClick.RemoveAllListeners();
                useItemButton.onClick.AddListener(UseSelectedItem);
            }

            RefreshUI();
            ClearItemInfo();
        }

        private void OnDisable()
        {
            if (inventoryManager != null)
            {
                inventoryManager.InventoryChanged -= HandleInventoryChanged;
            }
        }

        public void RefreshUI()
        {
            EnsureRuntimeLayout();

            for (int i = 0; i < spawnedSlots.Count; i++)
            {
                if (spawnedSlots[i] != null)
                {
                    Destroy(spawnedSlots[i]);
                }
            }

            spawnedSlots.Clear();

            IReadOnlyList<InventoryItemStack> items = InventoryManager.EnsureRuntimeInstance().Items;
            for (int i = 0; i < items.Count; i++)
            {
                Button slotButton = CreateRuntimeSlot(contentRoot);
                ItemSlotUI slotUI = slotButton.GetComponent<ItemSlotUI>();
                slotUI.Setup(items[i], SelectItem);
                spawnedSlots.Add(slotButton.gameObject);
            }
        }

        public void SelectItem(InventoryItemStack itemStack)
        {
            selectedItem = itemStack;

            if (selectedItem == null || selectedItem.itemData == null)
            {
                ClearItemInfo();
                return;
            }

            if (itemNameText != null)
            {
                itemNameText.text = selectedItem.itemData.itemName;
            }

            if (itemAmountText != null)
            {
                itemAmountText.text = $"Amount: {selectedItem.amount}";
            }

            if (itemDescriptionText != null)
            {
                itemDescriptionText.text = string.IsNullOrWhiteSpace(selectedItem.itemData.description)
                    ? "No description."
                    : selectedItem.itemData.description;
            }

            Debug.Log($"[PanelBagUI] Selected item: {selectedItem.itemData.itemName} x{selectedItem.amount}");
        }

        public void ClearItemInfo()
        {
            selectedItem = null;

            if (itemNameText != null)
            {
                itemNameText.text = "Select Item";
            }

            if (itemAmountText != null)
            {
                itemAmountText.text = "Amount: -";
            }

            if (itemDescriptionText != null)
            {
                itemDescriptionText.text = "Choose an item from the bag.";
            }
        }

        public void UseSelectedItem()
        {
            if (selectedItem == null)
            {
                Debug.Log("[PanelBagUI] Use item skipped because no item is selected.");
                return;
            }

            bool used = InventoryManager.EnsureRuntimeInstance().TryUseItem(selectedItem);
            if (used)
            {
                Debug.Log($"[PanelBagUI] Click use item: {selectedItem.itemData.itemName}");
                ClearItemInfo();
            }
        }

        private void HandleInventoryChanged()
        {
            RefreshUI();
        }

        private void EnsureRuntimeLayout()
        {
            RectTransform panelRect = GetComponent<RectTransform>();
            if (panelRect == null)
            {
                panelRect = gameObject.AddComponent<RectTransform>();
            }

            panelRect.anchorMin = new Vector2(0.15f, 0.16f);
            panelRect.anchorMax = new Vector2(0.85f, 0.82f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            Image panelImage = GetComponent<Image>();
            if (panelImage == null)
            {
                panelImage = gameObject.AddComponent<Image>();
            }

            panelImage.color = new Color(0.1f, 0.15f, 0.22f, 0.96f);

            if (contentRoot == null)
            {
                contentRoot = CreateListRoot(transform);
            }

            if (itemNameText == null || itemAmountText == null || itemDescriptionText == null || useItemButton == null)
            {
                CreateOrBindDetailArea(transform);
            }

            if (itemSlotTemplate == null)
            {
                itemSlotTemplate = CreateRuntimeSlot(contentRoot);
                itemSlotTemplate.gameObject.name = "ItemSlotTemplate";
                itemSlotTemplate.gameObject.SetActive(false);
            }
        }

        private static GameObject CreateRuntimePanelObject()
        {
            GameObject canvasObject = GameObject.Find("Canvas_HUD");
            Transform parent = canvasObject != null ? canvasObject.transform : null;

            GameObject panelObject = new GameObject("Panel_Bag", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            if (parent != null)
            {
                panelObject.transform.SetParent(parent, false);
            }

            return panelObject;
        }

        private RectTransform CreateListRoot(Transform parent)
        {
            GameObject listObject = new GameObject("ItemList", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(VerticalLayoutGroup));
            listObject.transform.SetParent(parent, false);

            RectTransform rect = listObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.03f, 0.14f);
            rect.anchorMax = new Vector2(0.48f, 0.94f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image image = listObject.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f, 0.08f);

            VerticalLayoutGroup layout = listObject.GetComponent<VerticalLayoutGroup>();
            layout.spacing = 8f;
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.childControlHeight = false;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;

            return rect;
        }

        private void CreateOrBindDetailArea(Transform parent)
        {
            GameObject detailObject = new GameObject("ItemDetail", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            detailObject.transform.SetParent(parent, false);

            RectTransform detailRect = detailObject.GetComponent<RectTransform>();
            detailRect.anchorMin = new Vector2(0.53f, 0.14f);
            detailRect.anchorMax = new Vector2(0.97f, 0.94f);
            detailRect.offsetMin = Vector2.zero;
            detailRect.offsetMax = Vector2.zero;

            Image detailImage = detailObject.GetComponent<Image>();
            detailImage.color = new Color(1f, 1f, 1f, 0.08f);

            itemNameText = CreateText("Txt_ItemName", detailObject.transform, new Vector2(0.5f, 0.85f), 26, FontStyle.Bold);
            itemAmountText = CreateText("Txt_ItemAmount", detailObject.transform, new Vector2(0.5f, 0.68f), 20, FontStyle.Normal);
            itemDescriptionText = CreateText("Txt_ItemDesc", detailObject.transform, new Vector2(0.5f, 0.46f), 18, FontStyle.Normal);
            itemDescriptionText.alignment = TextAnchor.UpperLeft;
            itemDescriptionText.horizontalOverflow = HorizontalWrapMode.Wrap;
            itemDescriptionText.verticalOverflow = VerticalWrapMode.Overflow;

            useItemButton = CreateButton("Btn_UseItem", detailObject.transform, new Vector2(0.5f, 0.12f), new Vector2(180f, 48f), "Use Item");
        }

        private Button CreateRuntimeSlot(RectTransform parent)
        {
            GameObject slotObject = new GameObject("ItemSlot", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(LayoutElement), typeof(ItemSlotUI));
            slotObject.transform.SetParent(parent, false);

            RectTransform rect = slotObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0f, 56f);

            Image background = slotObject.GetComponent<Image>();
            background.color = new Color(1f, 1f, 1f, 0.12f);

            LayoutElement layoutElement = slotObject.GetComponent<LayoutElement>();
            layoutElement.preferredHeight = 56f;

            CreateText("Txt_Name", slotObject.transform, new Vector2(0.32f, 0.5f), 18, FontStyle.Bold);
            Text amountText = CreateText("Txt_Amount", slotObject.transform, new Vector2(0.84f, 0.5f), 16, FontStyle.Normal);
            amountText.alignment = TextAnchor.MiddleRight;

            ItemSlotUI slotUI = slotObject.GetComponent<ItemSlotUI>();
            slotUI.BindRuntimeReferences(slotObject.transform.Find("Txt_Name")?.GetComponent<Text>(), amountText, null);
            return slotObject.GetComponent<Button>();
        }

        private static Text CreateText(string objectName, Transform parent, Vector2 anchor, int fontSize, FontStyle fontStyle)
        {
            GameObject textObject = new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            textObject.transform.SetParent(parent, false);

            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(220f, 40f);
            rect.anchoredPosition = Vector2.zero;

            Text text = textObject.GetComponent<Text>();
            text.font = GetRuntimeFont();
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = string.Empty;
            return text;
        }

        private static Button CreateButton(string objectName, Transform parent, Vector2 anchor, Vector2 size, string label)
        {
            GameObject buttonObject = new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);

            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.sizeDelta = size;

            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.28f, 0.52f, 0.24f, 1f);

            CreateText("Label", buttonObject.transform, new Vector2(0.5f, 0.5f), 18, FontStyle.Bold).text = label;
            return buttonObject.GetComponent<Button>();
        }

        private static Font GetRuntimeFont()
        {
            Font font = Font.CreateDynamicFontFromOSFont("Arial", 18);
            if (font != null)
            {
                return font;
            }

            return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }
    }
}
