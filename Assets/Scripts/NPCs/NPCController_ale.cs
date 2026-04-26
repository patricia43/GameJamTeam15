using UnityEngine;

public class NPCController_ale : MonoBehaviour
{
    public NPCDialogueState currentDState;
    //public NPCState currentState;
    public NPCDialogue[] dialogueSets;

    private int currentDialogueIndex = 0;

    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        // EnterBar();
    }

    NPCDialogue GetCurrentDialogue()
    {
        if (dialogueSets == null || dialogueSets.Length == 0)
            return null;

        if (currentDialogueIndex >= dialogueSets.Length)
            currentDialogueIndex = dialogueSets.Length - 1; // stay at last

        return dialogueSets[currentDialogueIndex];
    }

    public void AdvanceDialogueSet()
    {
        if (dialogueSets == null || dialogueSets.Length == 0)
            return;

        currentDialogueIndex++;

        if (currentDialogueIndex >= dialogueSets.Length)
            currentDialogueIndex = dialogueSets.Length - 1; // clamp
    }

    public void EnterBar()
    {
        currentDState = NPCDialogueState.Approaching;

        var dialogue = GetCurrentDialogue();
        if (dialogue != null)
            dialogueManager.StartDialogue(dialogue.introDialogue);
    }

    public void ReceiveDrink()
    {
        currentDState = NPCDialogueState.Drinking;

        var dialogue = GetCurrentDialogue();
        if (dialogue != null)
            dialogueManager.StartDialogue(dialogue.drinkDialogue);
    }

    public void LeaveBar()
    {
        currentDState = NPCDialogueState.Leaving;

        if (dialogueSets == null || dialogueSets.Length == 0)
            return;

        int randomIndex = Random.Range(0, dialogueSets.Length);
        var dialogue = dialogueSets[randomIndex];

        if (dialogue != null)
            dialogueManager.StartDialogue(dialogue.exitDialogue);
    }

    public void DropDrink()
    {
        currentDState = NPCDialogueState.Drinking;

        var dialogue = GetCurrentDialogue();
        if (dialogue != null)
            dialogueManager.StartDialogue(dialogue.dropDialogue);
    }
}
