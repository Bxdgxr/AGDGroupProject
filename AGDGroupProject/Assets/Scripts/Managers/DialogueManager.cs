using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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

    private DialogueTrigger currentTrigger;
    private int currentIndex;
    private Coroutine typingCoroutine;
    private UnityEngine.Events.UnityEvent pendingOnLineEnd;

    public bool IsDialogueActive => dialogueUI.activeSelf;

    void Update()
    {
        if (dialogueUI.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            AdvanceDialogue();
        }
    }

    public void StartDialogue(DialogueTrigger trigger)
    {
        currentTrigger = trigger;
        currentIndex = 0;
        dialogueUI.SetActive(true);

        playerPortrait.gameObject.SetActive(false);
        npcPortrait.gameObject.SetActive(false);

        AdvanceDialogue();
    }

    void AdvanceDialogue()
    {
        // Cancel any ongoing typing and prevent its onLineEnd from firing
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            pendingOnLineEnd = null;
        }

        if (currentIndex >= currentTrigger.lines.Length)
        {
            EndDialogue();
            return;
        }

        var line = currentTrigger.lines[currentIndex];
        var speaker = line.speaker;

        if (speaker.isPlayer)
        {
            playerNameText.text = speaker.characterName;
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

        // Store this line's onLineEnd to only trigger after full typing
        pendingOnLineEnd = line.onLineEnd;
        typingCoroutine = StartCoroutine(TypeSentence(line.sentence));
        currentIndex++;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        typingCoroutine = null;

        // Only invoke if this sentence typed out fully
        pendingOnLineEnd?.Invoke();
        pendingOnLineEnd = null;
    }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);
        playerPortrait.gameObject.SetActive(false);
        npcPortrait.gameObject.SetActive(false);
        currentTrigger = null;
        pendingOnLineEnd = null;
        typingCoroutine = null;
    }
}
