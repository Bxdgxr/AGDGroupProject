using UnityEngine;

public class ItemTester : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Sprite goldSprite;
    public Sprite swordSprite;

    void Start()
    {
        InventoryItem gold = new InventoryItem("Gold", "Shiny coins", goldSprite, true, 42);
        inventoryManager.AddItem(gold);

        InventoryItem sword = new InventoryItem("Sword", "A sharp blade", swordSprite, false);
        inventoryManager.AddItem(sword);
    }
    void Update()
{
    if (Input.GetKeyDown(KeyCode.G))
    {
        InventoryItem gold = new InventoryItem("Gold", "Shiny coins", goldSprite, true, 25);
        inventoryManager.AddItem(gold);
    }

    if (Input.GetKeyDown(KeyCode.S))
    {
        InventoryItem sword = new InventoryItem("Sword", "A sharp blade", swordSprite, false);
        inventoryManager.AddItem(sword);
    }
}

}
