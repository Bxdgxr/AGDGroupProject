using UnityEngine;

public class GoldCheckDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueTrigger goldDialogue;

    [Header("Gold Reference")]
    public InventoryItemData goldItemData;  // <- Drag your gold ScriptableObject here

    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered && GetGoldAmount() >= 100)
        {
            dialogueManager.StartDialogue(goldDialogue);
            hasTriggered = true;
        }
    }

    int GetGoldAmount()
    {
        int total = 0;
        foreach (var slot in FindAnyObjectByType<InventoryManager>().slots)
        {
            var item = slot.GetItem();
            if (item != null && item.itemName == goldItemData.itemName) // safer to compare a unique name
            {
                total += item.quantity;
            }
        }
        return total;
    }
}
