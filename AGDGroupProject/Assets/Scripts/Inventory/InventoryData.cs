using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
    public static InventoryData Instance;

    [System.Serializable]
    public class StoredItem
    {
        public string itemName;
        public string description;
        public Sprite icon;
        public bool isStackable;
        public int quantity;
        public ItemCategory category;
    }

    public List<StoredItem> savedInventory = new();
    public List<StoredItem> savedHotbar = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Save inventory slots
    public void SaveInventory(List<InventorySlot> slots)
    {
        savedInventory.Clear();

        foreach (var slot in slots)
        {
            var item = slot.GetItem();
            if (item != null)
            {
                savedInventory.Add(new StoredItem
                {
                    itemName = item.itemName,
                    description = item.description,
                    icon = item.icon,
                    isStackable = item.isStackable,
                    quantity = item.quantity
                });
            }
        }
    }

    // Load inventory slots
    public void LoadInventory(List<InventorySlot> slots)
    {
        for (int i = 0; i < slots.Count; i++)
            slots[i].ClearSlot();

        for (int i = 0; i < savedInventory.Count && i < slots.Count; i++)
        {
            var stored = savedInventory[i];
            var newItem = new InventoryItem(
                stored.itemName,
                stored.description,
                stored.icon,
                stored.isStackable,
                stored.quantity
            );
            slots[i].SetItem(newItem);
        }
    }

    // Save hotbar slots
    public void SaveHotbar(List<HotbarSlotUI> hotbarSlots)
    {
        savedHotbar.Clear();

        foreach (var slot in hotbarSlots)
        {
            if (slot.HasItem())
            {
                savedHotbar.Add(new StoredItem
                {
                    itemName = slot.itemData.itemName,
                    description = slot.itemData.description,
                    icon = slot.itemData.icon,
                    isStackable = slot.itemData.isStackable,
                    quantity = slot.quantity,
                    category = slot.itemData.category 
                });
            }
        }
    }

    // Load hotbar slots
public void LoadHotbar(List<HotbarSlotUI> hotbarSlots)
{
    for (int i = 0; i < hotbarSlots.Count; i++)
        hotbarSlots[i].Clear();

    for (int i = 0; i < savedHotbar.Count && i < hotbarSlots.Count; i++)
    {
        var stored = savedHotbar[i];

        var data = ScriptableObject.CreateInstance<InventoryItemData>();
        data.itemName = stored.itemName;
        data.description = stored.description;
        data.icon = stored.icon;
        data.isStackable = stored.isStackable;
        data.category = stored.category;

        hotbarSlots[i].SetSlot(data, stored.quantity);
    }
}

    public void ClearAll()
{
    savedInventory.Clear();
    savedHotbar.Clear();
}

}
