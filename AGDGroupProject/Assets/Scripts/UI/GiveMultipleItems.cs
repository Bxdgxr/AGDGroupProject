using UnityEngine;

public class GiveMultipleItems : MonoBehaviour
{
    [System.Serializable]
    public class ItemEntry
    {
        public InventoryItemData itemData;
        public int quantity = 1;
    }

    public InventoryManager inventoryManager;
    public HotbarManager hotbarManager; // Reference to the HotbarManager
    public ItemPopUpManager itemPopUpManager;
    public ItemEntry[] items;

    private bool given = false;
    public void GiveAllToPlayer()
    {
        if (given) return;
        given = true;

        foreach (var entry in items)
        {
            InventoryItem item = new InventoryItem(
                entry.itemData.itemName,
                entry.itemData.description,
                entry.itemData.icon,
                entry.itemData.isStackable,
                entry.quantity
            );

            inventoryManager.AddItem(item);
            itemPopUpManager.ShowPopUp(entry.itemData.icon, entry.itemData.itemName, entry.quantity);
            hotbarManager.AssignItem(entry.itemData, entry.quantity);
        }
    }
}
