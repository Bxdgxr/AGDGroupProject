using UnityEngine;
using TMPro;

public class NameEntryUI : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void SetPlayerName()
    {
        string input = nameInput.text.Trim();
        if (!string.IsNullOrEmpty(input))
        {
            PlayerProfile.Instance.playerName = input;
            Debug.Log("Player name set to: " + input);
        }
    }
}
