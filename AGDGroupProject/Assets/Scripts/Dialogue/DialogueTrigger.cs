using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public DialogueCharacter speaker;
        [TextArea] public string sentence;
        public UnityEvent onLineEnd;
        public DialogueChoice[] choices;
        public bool waitForEventToFinish; // ✅ new toggle
    }

    public DialogueLine[] lines;
}
