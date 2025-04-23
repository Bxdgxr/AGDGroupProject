using UnityEngine;

public class GiveItem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemPopUpManager itemPopUpManager;
    public InventoryItemData itemData;
    public HotbarManager hotbarManager; // Reference to the HotbarManager
    public int quantity = 1;
    private bool given = false;

    public void GiveToPlayer()
    {
        if (given) return;
        given = true;

        InventoryItem item = new InventoryItem(
            itemData.itemName,
            itemData.description,
            itemData.icon,
            itemData.isStackable,
            quantity
        );

        inventoryManager.AddItem(item);
        itemPopUpManager.ShowPopUp(itemData.icon, itemData.itemName, quantity);
        hotbarManager.AssignItem(itemData, quantity);
    }
}
