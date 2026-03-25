using System;
using System.Collections.Generic;
using SpiritBond.Core;
using UnityEngine;

namespace SpiritBond.Inventory
{
    public class InventoryManager : SingletonBehaviour<InventoryManager>
    {
        private readonly List<InventoryItemStack> items = new List<InventoryItemStack>();

        public event Action InventoryChanged;

        public IReadOnlyList<InventoryItemStack> Items => items;

        protected override bool DontDestroyOnLoadEnabled => true;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
            {
                return;
            }

            SeedTestItemsIfNeeded();
        }

        public static InventoryManager EnsureRuntimeInstance()
        {
            if (Instance != null)
            {
                return Instance;
            }

            GameObject runtimeObject = new GameObject("InventoryManager");
            return runtimeObject.AddComponent<InventoryManager>();
        }

        public void SeedTestItemsIfNeeded()
        {
            if (items.Count > 0)
            {
                return;
            }

            // Test data only. Real item asset hookup from content pipeline is not use yet.
            AddItem(CreateRuntimeItem("Potion", "Restore a small amount of HP.", true), 5, false);
            AddItem(CreateRuntimeItem("Ether", "Restore a small amount of energy.", true), 3, false);
            AddItem(CreateRuntimeItem("Quest Scroll", "A key item used for testing Bag UI.", false), 1, false);
            NotifyInventoryChanged();
        }

        public void AddItem(ItemData itemData, int amount = 1, bool notify = true)
        {
            if (itemData == null || amount <= 0)
            {
                return;
            }

            InventoryItemStack existingStack = FindStack(itemData);
            if (existingStack != null)
            {
                existingStack.amount += amount;
            }
            else
            {
                items.Add(new InventoryItemStack(itemData, amount));
            }

            if (notify)
            {
                NotifyInventoryChanged();
            }
        }

        public bool TryUseItem(InventoryItemStack itemStack)
        {
            if (itemStack == null || itemStack.itemData == null || itemStack.amount <= 0)
            {
                Debug.LogWarning("[InventoryManager] TryUseItem failed because item stack is invalid.");
                return false;
            }

            Debug.Log($"[InventoryManager] Use item: {itemStack.itemData.itemName} x1");

            if (itemStack.itemData.consumable)
            {
                itemStack.amount--;
                if (itemStack.amount <= 0)
                {
                    items.Remove(itemStack);
                }

                NotifyInventoryChanged();
            }

            return true;
        }

        private InventoryItemStack FindStack(ItemData itemData)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null && items[i].itemData == itemData)
                {
                    return items[i];
                }
            }

            return null;
        }

        private void NotifyInventoryChanged()
        {
            InventoryChanged?.Invoke();
        }

        private static ItemData CreateRuntimeItem(string itemName, string description, bool consumable)
        {
            ItemData itemData = ScriptableObject.CreateInstance<ItemData>();
            itemData.itemName = itemName;
            itemData.description = description;
            itemData.consumable = consumable;
            return itemData;
        }
    }
}
