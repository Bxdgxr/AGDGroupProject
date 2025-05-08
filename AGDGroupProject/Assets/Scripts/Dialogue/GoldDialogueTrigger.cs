using UnityEngine;

public class GoldDialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("Gold Requirement")]
    public InventoryItemData goldItemData; // Your gold ScriptableObject
    public int requiredGold = 100;

    [Header("Dialogues")]
    public DialogueTrigger enoughGoldDialogue;     // Triggered if player has enough gold
    public DialogueTrigger notEnoughGoldDialogue;  // Triggered otherwise

    [Header("Settings")]
    public bool requireInteraction = true;

    private bool isPlayerInRange = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }

    void Update()
    {
        if (isPlayerInRange && requireInteraction && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueManager.IsDialogueActive)
                TriggerDialogue();
        }
    }

    void TriggerDialogue()
    {
        if (GetGoldAmount() >= requiredGold && enoughGoldDialogue != null)
        {
            dialogueManager.StartDialogue(enoughGoldDialogue);
        }
        else if (notEnoughGoldDialogue != null)
        {
            dialogueManager.StartDialogue(notEnoughGoldDialogue);
        }
    }

    int GetGoldAmount()
    {
        int total = 0;
        foreach (var slot in FindAnyObjectByType<InventoryManager>().slots)
        {
            var item = slot.GetItem();
            if (item != null && item.itemName == goldItemData.itemName)
            {
                total += item.quantity;
            }
        }
        return total;
    }
}
