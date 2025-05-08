using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueUI;
    public TMP_Text dialogueText;
    public TMP_Text playerNameText;
    public TMP_Text npcNameText;
    public Image playerPortrait;
    public Image npcPortrait;
    public float textSpeed = 0.03f;
    public GameObject continuePromptText;

    [Header("Choices")]
    public GameObject choiceContainer;
    public GameObject choiceButtonPrefab;

    [Header("Player Control")]
    public PlayerMovement playerMovement; // ✅ Drag your player script here in Inspector

    private DialogueTrigger currentTrigger;
    private int currentIndex;
    private Coroutine typingCoroutine;
    private UnityEvent pendingOnLineEnd;

    private bool canContinue = false;
    private bool waitingForEvent = false;

    public bool IsDialogueActive => dialogueUI.activeSelf;

    void Update()
    {
        if (dialogueUI.activeSelf && Input.GetKeyDown(KeyCode.E) && typingCoroutine == null && canContinue && !choiceContainer.activeSelf && !DialogueRumpleEnd.IsInputActive)
        {
            AdvanceDialogue();
        }
    }

    public void StartDialogue(DialogueTrigger trigger)
    {
        currentTrigger = trigger;
        currentIndex = 0;
        dialogueUI.SetActive(true);
        choiceContainer.SetActive(false);
        ClearChoices();

        canContinue = false;
        waitingForEvent = false;
        if (continuePromptText != null)
            continuePromptText.SetActive(false);

if (playerMovement != null)
{
    playerMovement.enabled = false; // Freeze input

    // Stop Rigidbody movement
    Rigidbody2D rb = playerMovement.GetComponent<Rigidbody2D>();
    if (rb != null)
        rb.linearVelocity = Vector2.zero;

    // Stop animation
    Animator anim = playerMovement.GetComponent<Animator>();
    if (anim != null)
        anim.SetBool("IsMoving", false);
}

        AdvanceDialogue();
    }

    void AdvanceDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            pendingOnLineEnd?.Invoke();
            pendingOnLineEnd = null;
        }

        canContinue = false;
        waitingForEvent = false;
        if (continuePromptText != null)
            continuePromptText.SetActive(false);

        if (currentIndex >= currentTrigger.lines.Length)
        {
            EndDialogue();
            return;
        }

        var line = currentTrigger.lines[currentIndex];
        var speaker = line.speaker;

        if (speaker.isPlayer)
        {
            playerNameText.text = PlayerProfile.Instance != null ? PlayerProfile.Instance.playerName : speaker.characterName;
            playerPortrait.sprite = speaker.portrait;
            playerPortrait.gameObject.SetActive(true);
            npcNameText.text = "";
            npcPortrait.gameObject.SetActive(false);
        }
        else
        {
            npcNameText.text = speaker.characterName;
            npcPortrait.sprite = speaker.portrait;
            npcPortrait.gameObject.SetActive(true);
            playerNameText.text = "";
            playerPortrait.gameObject.SetActive(false);
        }

        pendingOnLineEnd = line.onLineEnd;
        typingCoroutine = StartCoroutine(TypeSentence(line.sentence, line.waitForEventToFinish));

        currentIndex++;
    }

    IEnumerator TypeSentence(string sentence, bool waitForEvent)
    {
        dialogueText.text = "";

        // Replace {player} with player's chosen name
        string processed = sentence.Replace("{player}", PlayerProfile.Instance != null ? PlayerProfile.Instance.playerName : "???");

        foreach (char letter in processed.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        typingCoroutine = null;
        pendingOnLineEnd?.Invoke();
        pendingOnLineEnd = null;

        var line = currentTrigger.lines[currentIndex - 1];

        if (line.choices != null && line.choices.Length > 0)
        {
            ShowChoices(line.choices);
        }
        else if (waitForEvent)
        {
            waitingForEvent = true; // Wait until cutscene/event finishes
        }
        else
        {
            StartCoroutine(EnableContinuePrompt());
        }
    }

    public void NotifyLineFinished()
    {
        if (waitingForEvent)
        {
            waitingForEvent = false;
            StartCoroutine(EnableContinuePrompt());
        }
    }

    private IEnumerator EnableContinuePrompt()
    {
        yield return new WaitForSeconds(0.1f);

        canContinue = true;
        if (continuePromptText != null)
            continuePromptText.SetActive(true);
    }

    void ShowChoices(DialogueChoice[] choices)
    {
        ClearChoices();
        choiceContainer.SetActive(true);

        foreach (DialogueChoice choice in choices)
        {
            GameObject choiceGO = Instantiate(choiceButtonPrefab, choiceContainer.transform);
            TMP_Text buttonText = choiceGO.GetComponentInChildren<TMP_Text>();
            buttonText.text = choice.choiceText;

            Button btn = choiceGO.GetComponent<Button>();
            btn.onClick.AddListener(() => OnChoiceSelected(choice.nextDialogue));
        }
    }

    void OnChoiceSelected(DialogueTrigger nextDialogue)
    {
        ClearChoices();
        if (nextDialogue != null)
        {
            StartDialogue(nextDialogue);
        }
        else
        {
            EndDialogue();
        }
    }

    void ClearChoices()
    {
        foreach (Transform child in choiceContainer.transform)
        {
            Destroy(child.gameObject);
        }
        choiceContainer.SetActive(false);
    }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);
        playerPortrait.gameObject.SetActive(false);
        npcPortrait.gameObject.SetActive(false);
        currentTrigger = null;
        pendingOnLineEnd = null;
        typingCoroutine = null;
        ClearChoices();

        canContinue = false;
        waitingForEvent = false;
        if (continuePromptText != null)
            continuePromptText.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true; // ✅ Unfreeze player
    }
}