using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueNameCheckUI : MonoBehaviour
{
    public GameObject nameCheckPanel;
    public Image visualImage; // assign your non-fullscreen image
    public TMP_InputField nameInputField;
    public TMP_Text feedbackText;

    public DialogueTrigger correctDialogue;
    public DialogueTrigger incorrectDialogue;

    public string[] wrongResponses;

    private DialogueManager dialogueManager;

    public static bool IsInputActive { get; private set; }

    void Start()
    {
        nameCheckPanel.SetActive(false);
        dialogueManager = FindAnyObjectByType<DialogueManager>();
    }

    public void StartNameCheck()
    {
        IsInputActive = true;
        nameCheckPanel.SetActive(true);
        feedbackText.text = "";
        nameInputField.text = "";
        nameInputField.ActivateInputField();

        nameInputField.onSubmit.RemoveAllListeners();
        nameInputField.onSubmit.AddListener(_ => CheckName());
    }

    void CheckName()
    {
        string typed = nameInputField.text.Trim().ToLower();
        string expected = PlayerProfile.Instance.playerName.Trim().ToLower();

        if (typed == expected)
        {
            nameCheckPanel.SetActive(false);
            IsInputActive = false;
            dialogueManager.StartDialogue(correctDialogue);
        }
        else
        {
            string message = wrongResponses.Length > 0 ? wrongResponses[Random.Range(0, wrongResponses.Length)] : "Try again.";
            feedbackText.text = message;
            nameInputField.text = "";
            nameInputField.ActivateInputField();
        }
    }
}
