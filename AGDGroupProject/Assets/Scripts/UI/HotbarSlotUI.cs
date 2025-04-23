using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text quantityText;

    [HideInInspector] public InventoryItemData itemData;
    [HideInInspector] public int quantity;

    public void SetSlot(InventoryItemData data, int amount)
    {
        itemData = data;
        quantity = amount;

        iconImage.sprite = data.icon;
        iconImage.enabled = true;

        if (data.isStackable && quantity > 1)
            quantityText.text = quantity.ToString();
        else
            quantityText.text = "";

        gameObject.SetActive(true);
    }

    public void UpdateQuantity(int newAmount)
    {
        quantity = newAmount;
        quantityText.text = (itemData.isStackable && quantity > 1) ? quantity.ToString() : "";
    }

    public void Clear()
    {
        itemData = null;
        quantity = 0;
        iconImage.enabled = false;
        quantityText.text = "";
    }

    public bool HasItem() => itemData != null;
}
