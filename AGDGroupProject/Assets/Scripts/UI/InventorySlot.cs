using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI countText;

    private InventoryItem currentItem;
    private int itemCount;

public void SetItem(InventoryItem item, int count = 1)
{
    currentItem = item;
    itemCount = count;
    iconImage.sprite = item.icon;
    iconImage.enabled = true;                          // Show icon
    countText.text = item.isStackable ? $"x{itemCount}" : "";
    GetComponent<Button>().interactable = true;
}


    public void AddOne()
    {
        itemCount++;
        countText.text = $"x{itemCount}";
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemCount = 0;
        iconImage.enabled = false;
        countText.text = "";
        GetComponent<Button>().interactable = false;
    }

    public void OnClick()
    {
        if (currentItem != null)
        {
            FindFirstObjectByType<InventoryUI>().ShowDescription(currentItem);

        }
    }

    public bool HasItem(InventoryItem item) => currentItem == item;
}
