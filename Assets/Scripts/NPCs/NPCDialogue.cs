using UnityEngine;
[CreateAssetMenu(fileName = "NPCDialogue", menuName = "Dialogue/NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public DialogueLines[] introDialogue;
    public DialogueLines[] drinkDialogue;
    public DialogueLines[] exitDialogue;
}
