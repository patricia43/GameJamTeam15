using UnityEngine;

[System.Serializable]
public class DialogueLines
{
    public string NPCName;

    [TextArea(2, 5)]
    public string text;
}
