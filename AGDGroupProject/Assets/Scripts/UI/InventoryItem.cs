using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public string description;
    public Sprite icon;
    public bool isStackable;
    public int quantity;

    public InventoryItem(string name, string desc, Sprite icon, bool stackable, int quantity = 1)
    {
        this.itemName = name;
        this.description = desc;
        this.icon = icon;
        this.isStackable = stackable;
        this.quantity = quantity;
    }
}
