using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

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

    [Header("Choices")]
    public GameObject choiceContainer; // Panel that holds buttons
    public GameObject choiceButtonPrefab; // Prefab with TMP_Text & Button

    private DialogueTrigger currentTrigger;
    private int currentIndex;
    private Coroutine typingCoroutine;
    private UnityEvent pendingOnLineEnd;

    public bool IsDialogueActive => dialogueUI.activeSelf;

    void Update()
    {
        if (dialogueUI.activeSelf && Input.GetKeyDown(KeyCode.E) && typingCoroutine == null && choiceContainer.activeSelf == false)
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

        playerPortrait.gameObject.SetActive(false);
        npcPortrait.gameObject.SetActive(false);

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

        pendingOnLineEnd = line.onLineEnd;
        typingCoroutine = StartCoroutine(TypeSentence(line.sentence));

        // Choices are shown AFTER the sentence types out, in coroutine
        if (line.choices != null && line.choices.Length > 0)
        {
            currentIndex++; // Still increase index
        }
        else
        {
            currentIndex++;
        }
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
        pendingOnLineEnd?.Invoke();
        pendingOnLineEnd = null;

        var line = currentTrigger.lines[currentIndex - 1];
        if (line.choices != null && line.choices.Length > 0)
        {
            ShowChoices(line.choices);
        }
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
    }
}
