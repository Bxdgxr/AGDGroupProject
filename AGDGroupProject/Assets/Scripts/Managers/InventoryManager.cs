using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> slots;
    public Image detailIcon;
    public TMP_Text detailName;
    public TMP_Text detailDescription;
    public GameObject trashcanButton;

    private InventorySlot selectedSlot;

    public void Start()
    {
        foreach (var slot in slots)
            slot.Init(this);

        ShowItemDetails(null);
    }

    public void AddItem(InventoryItem newItem)
    {
        // Stackable check
        if (newItem.isStackable)
        {
            foreach (var slot in slots)
            {
                var item = slot.GetItem();
                if (item != null && item.itemName == newItem.itemName && item.quantity < 99)
                {
                    int spaceLeft = 99 - item.quantity;
                    int toAdd = Mathf.Min(spaceLeft, newItem.quantity);
                    item.quantity += toAdd;
                    slot.UpdateQuantity(item.quantity);
                    newItem.quantity -= toAdd;

                    if (newItem.quantity <= 0)
                        return;
                }
            }
        }

        // Add to empty slots
        foreach (var slot in slots)
        {
            if (slot.GetItem() == null)
            {
                InventoryItem copy = new InventoryItem(
                    newItem.itemName,
                    newItem.description,
                    newItem.icon,
                    newItem.isStackable,
                    newItem.isStackable ? Mathf.Min(99, newItem.quantity) : 1
                );
                slot.SetItem(copy);
                newItem.quantity -= copy.quantity;
                if (newItem.quantity <= 0)
                    break;
            }
        }
    }

    public void ShowItemDetails(InventorySlot slot)
    {
        selectedSlot = slot;

        if (slot == null || slot.GetItem() == null)
        {
            detailIcon.enabled = false;
            detailName.text = "";
            detailDescription.text = "";
            trashcanButton.SetActive(false);
            return;
        }

        var item = slot.GetItem();
        detailIcon.enabled = true;
        detailIcon.sprite = item.icon;
        detailName.text = item.itemName;
        detailDescription.text = item.description;
        trashcanButton.SetActive(true);
    }

    public void RemoveSelectedItem()
    {
        if (selectedSlot != null)
        {
            selectedSlot.ClearSlot();
            ShowItemDetails(null);
        }
    }
}
