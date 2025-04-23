using UnityEngine;
using TMPro;

public class DialogueRumpleEnd : MonoBehaviour
{
    [Header("Input UI")]
    public GameObject inputPanel;
    public TMP_InputField answerInput;

    [Header("Answer Settings")]
    public string correctAnswer;
    public DialogueTrigger successDialogue;
    public DialogueTrigger failureDialogue;

    private int attemptsLeft = 3;
    private DialogueManager dialogueManager;

    void Start()
    {
       dialogueManager = FindAnyObjectByType<DialogueManager>();
        inputPanel.SetActive(false);
    }

    public void StartInput()
    {
        inputPanel.SetActive(true);
        answerInput.text = "";
        answerInput.ActivateInputField(); // Focus the input

        // Set up Enter key to trigger CheckAnswer when input is submitted
        answerInput.onSubmit.RemoveAllListeners();
        answerInput.onSubmit.AddListener(_ => CheckAnswer());
    }

    void CheckAnswer()
    {
        
        string input = answerInput.text.Trim().ToLower();
        string correct = correctAnswer.Trim().ToLower();

        if (input == correct)
        {
            inputPanel.SetActive(false);
            dialogueManager.StartDialogue(successDialogue);
        }
        else
        {
            attemptsLeft--;
            if (attemptsLeft <= 0)
            {
                inputPanel.SetActive(false);
                dialogueManager.StartDialogue(failureDialogue);
            }
            else
            {
                inputPanel.SetActive(false);
                StartInput(); // Show input again for retry
            }
        }
    }
}
