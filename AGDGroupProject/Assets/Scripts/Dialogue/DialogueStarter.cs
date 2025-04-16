using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("Main & Fallback Dialogue")]
    public DialogueTrigger firstTimeDialogue;
    public DialogueTrigger repeatDialogue;

    [Header("Trigger Settings")]
    public bool triggerOnEnter = true;
    public bool requireInteraction = false;

    private bool isPlayerInRange = false;
    private bool hasPlayedFirstTime = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInRange = true;

        if (triggerOnEnter && !dialogueManager.IsDialogueActive)
        {
            PlayDialogue();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        if (requireInteraction && isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueManager.IsDialogueActive)
            {
                PlayDialogue();
                isPlayerInRange = false; // Optional: stop multiple presses
            }
        }
    }

    void PlayDialogue()
    {
        if (!hasPlayedFirstTime && firstTimeDialogue != null)
        {
            dialogueManager.StartDialogue(firstTimeDialogue);
            hasPlayedFirstTime = true;
        }
        else if (repeatDialogue != null)
        {
            dialogueManager.StartDialogue(repeatDialogue);
        }
    }

    // Optional: allow external reset (e.g. from a quest system)
    public void ResetDialogue()
    {
        hasPlayedFirstTime = false;
    }
}
