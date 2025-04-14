using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Transform slotParent; // assign InventoryPanel
    public GameObject slotPrefab;
    public TextMeshProUGUI descriptionText;

    private List<InventorySlot> slots = new();

    public void AddItem(InventoryItem item)
    {
        // Check if already in inventory
        foreach (var slot in slots)
        {
            if (slot.HasItem(item) && item.isStackable)
            {
                slot.AddOne();
                return;
            }
        }

        // Add to new slot
        GameObject newSlotObj = Instantiate(slotPrefab, slotParent);
        InventorySlot newSlot = newSlotObj.GetComponent<InventorySlot>();
        slots.Add(newSlot);
        newSlot.SetItem(item);
        newSlot.GetComponent<Button>().onClick.AddListener(() => newSlot.OnClick());
    }

    public void ShowDescription(InventoryItem item)
    {
        descriptionText.text = $"{item.itemName}\n\n{item.description}";
    }
}
