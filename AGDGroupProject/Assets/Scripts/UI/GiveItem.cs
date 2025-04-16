using UnityEngine;
public class GiveItem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemPopUpManager itemPopUpManager;
    public InventoryItemData itemData;
    public int quantity = 1;

    public void GiveToPlayer()
    {
        InventoryItem item = new InventoryItem(
            itemData.itemName,
            itemData.description,
            itemData.icon,
            itemData.isStackable,
            quantity
        );

        inventoryManager.AddItem(item);
        itemPopUpManager.ShowPopUp(itemData.icon, itemData.itemName, quantity);
    }
}
