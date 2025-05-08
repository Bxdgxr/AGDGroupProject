using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text quantityText;
    public Button button;

    [Header("Selection Sprites")]
    public Sprite defaultSprite;
    public Sprite selectedSprite;

    private Image backgroundImage;

    [HideInInspector] public InventoryItemData itemData;
    [HideInInspector] public int quantity;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }

    public void SetSlot(InventoryItemData data, int amount)
    {
        itemData = data;
        quantity = amount;

        iconImage.sprite = data.icon;
        iconImage.enabled = true;

        quantityText.text = (data.isStackable && quantity > 1) ? quantity.ToString() : "";
        button.interactable = true;
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
        button.interactable = false;
        SetSelected(false);
    }

    public bool HasItem() => itemData != null;

    public void SetSelected(bool isSelected)
    {
        if (backgroundImage != null)
        {
            backgroundImage.sprite = isSelected ? selectedSprite : defaultSprite;
        }
    }
}
