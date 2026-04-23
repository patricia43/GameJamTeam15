using UnityEngine;
using System.Collections.Generic;

public enum NPCState
{
    // TBD
    Normal,
    Mild,
    Wild,
    Crazy,
    Insane,
    Dead
}

public enum DialogueTrigger
{
    Order,
    Hit,
    Drop,
    Goodbye
}

public enum Speaker
{
    NPC,
    Barman
}

[System.Serializable]
public class DialogueLine
{
    public Speaker speaker;

    [TextArea(2,5)]
    public string text;
}

[System.Serializable]
public class DialogueSequence
{
    public List<DialogueLine> lines;
}

[System.Serializable]
public class TriggerDialogueGroup
{
    public DialogueTrigger trigger;

    public List<DialogueSequence> sequences;
}

[CreateAssetMenu(menuName = "Bartender/NPC")]
public class NPCData : ScriptableObject
{
    public string npcName;

    public List<TriggerDialogueGroup> dialogueGroups;
}
