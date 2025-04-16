using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySlot : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TMP_Text quantityText;

    private InventoryItem currentItem;
    private InventoryManager manager;

    public void Init(InventoryManager inventoryManager)
    {
        manager = inventoryManager;
        button.interactable = false;
        button.onClick.AddListener(OnClick);
        ClearSlot();
    }

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;
        quantityText.text = item.isStackable && item.quantity > 1 ? "x" + item.quantity.ToString() : "";
        button.interactable = true;
    }

    public void UpdateQuantity(int newQuantity)
    {
        currentItem.quantity = newQuantity;
        quantityText.text = newQuantity > 1 ? newQuantity.ToString() : "";
    }

    public InventoryItem GetItem() => currentItem;

    public void ClearSlot()
    {
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
        button.interactable = false;
    }

    private void OnClick()
    {
        if (currentItem != null)
            manager.ShowItemDetails(this);
    }
}
