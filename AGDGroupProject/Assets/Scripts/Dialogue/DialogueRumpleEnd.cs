using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueRumpleEnd : MonoBehaviour
{
    [Header("Input UI")]
    public GameObject inputPanel;
    public TMP_InputField answerInput;

    [Header("Answer Settings")]
    public string correctAnswer;
    public DialogueTrigger successDialogue;
    public DialogueTrigger failureDialogue;
    public TMP_Text dialogueText; // assign from your DialogueManager UI
[Header("Wrong Answer Messages")]
public string[] wrongAnswerResponses = {
    "Hihihi. That's not quite right.",
    "Uh oh! Wrong answer.",
    "Wrong. Want to guess again?",
    "Nope. That's not correct."
};


    private int attemptsLeft = 3;
    private DialogueManager dialogueManager;
public static bool IsInputActive { get; private set; } = false;

    void Start()
    {
       dialogueManager = FindAnyObjectByType<DialogueManager>();
        inputPanel.SetActive(false);
    }

public void StartInput()
{
    IsInputActive = true;
    inputPanel.SetActive(true);
    answerInput.text = "";
    answerInput.ActivateInputField();

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
        IsInputActive = false; // ✅ Disable input mode
        dialogueManager.StartDialogue(successDialogue);
    }
   else
{
    attemptsLeft--;

    if (attemptsLeft <= 0)
    {
        inputPanel.SetActive(false);
        IsInputActive = false;
        dialogueManager.StartDialogue(failureDialogue);
    }
    else
    {
        inputPanel.SetActive(false);

        // 👇 Show a message before the next input round
        if (dialogueText != null)
        {
            if (dialogueText != null && wrongAnswerResponses.Length > 0)
{
    int i = Random.Range(0, wrongAnswerResponses.Length);
    dialogueText.text = wrongAnswerResponses[i];
}

        }

        // 🕒 Add a tiny delay before reopening the input
        StartCoroutine(ShowInputAgainAfterDelay(1.2f)); // 1.2 seconds is comfy
    }
}

}
IEnumerator ShowInputAgainAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    StartInput();
}


}
