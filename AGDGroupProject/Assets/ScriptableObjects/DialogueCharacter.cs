using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Dialogue/Character")]
public class DialogueCharacter : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    public bool isPlayer; // if true, show on the left
}
