using UnityEngine;

public enum ItemCategory
{
    None,
    Sword,
    Pickaxe,
    Potion,
    // You can add more later like Bow, Shield, Tool, Food, etc.
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public bool isStackable;

    public ItemCategory category; // ✅ Add this
}
