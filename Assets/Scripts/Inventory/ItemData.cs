using UnityEngine;

namespace SpiritBond.Inventory
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "SpiritBond/Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Item Info")]
        public string itemName;
        [TextArea] public string description;
        public Sprite icon;

        [Header("Usage")]
        public bool consumable = true;
    }
}
