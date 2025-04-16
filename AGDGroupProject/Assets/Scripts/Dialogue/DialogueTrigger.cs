using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public DialogueCharacter speaker;
        [TextArea] public string sentence;
        public UnityEvent onLineEnd; // Scene-referencable
    }

    public DialogueLine[] lines;
}
