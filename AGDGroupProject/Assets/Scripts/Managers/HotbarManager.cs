using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    public HotbarSlotUI swordSlot;
    public HotbarSlotUI pickaxeSlot;
    public HotbarSlotUI potionSlot;

    public InventoryItemData swordData;
    public InventoryItemData pickaxeData;
    public InventoryItemData potionData;

    private HotbarSlotUI[] slotArray;
    private int selectedIndex = -1;

    void Start()
    {
        slotArray = new[] { swordSlot, pickaxeSlot, potionSlot };

        // Assign click listeners
        swordSlot.GetComponent<Button>().onClick.AddListener(() => SelectSlot(0));
        pickaxeSlot.GetComponent<Button>().onClick.AddListener(() => SelectSlot(1));
        potionSlot.GetComponent<Button>().onClick.AddListener(() => SelectSlot(2));

        swordSlot.Clear();
        pickaxeSlot.Clear();
        potionSlot.Clear();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
    }

    public void AssignItem(InventoryItemData data, int quantity)
    {
switch (data.category)
{
    case ItemCategory.Sword:
        swordSlot.SetSlot(data, quantity);
        break;

    case ItemCategory.Pickaxe:
        pickaxeSlot.SetSlot(data, quantity);
        break;

    case ItemCategory.Potion:
        if (potionSlot.HasItem())
            potionSlot.UpdateQuantity(potionSlot.quantity + quantity);
        else
            potionSlot.SetSlot(data, quantity);
        break;

    default:
        Debug.Log($"Item {data.itemName} has no hotbar slot.");
        break;
}

    }

    public void SelectSlot(int index)
    {
        if (!slotArray[index].HasItem()) return;

        selectedIndex = index;
        Debug.Log("Selected hotbar slot: " + (index + 1) + " — " + slotArray[index].itemData.itemName);
    }
}
