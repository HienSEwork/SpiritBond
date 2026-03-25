using System;

namespace SpiritBond.Inventory
{
    [Serializable]
    public class InventoryItemStack
    {
        public ItemData itemData;
        public int amount;

        public InventoryItemStack(ItemData itemData, int amount)
        {
            this.itemData = itemData;
            this.amount = amount;
        }
    }
}
