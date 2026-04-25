using UnityEngine;

public class NPCController_ale : MonoBehaviour
{
    public NPCDialogueState currentDState;
    //public NPCState currentState;
    public NPCDialogue dialogueData;

    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        EnterBar();
    }

    public void EnterBar()
    {
        currentDState = NPCDialogueState.Approaching;

        if (dialogueData != null)
        {
            dialogueManager.StartDialogue(dialogueData.introDialogue);
        }
    }

    public void ReceiveDrink()
    {
        currentDState = NPCDialogueState.Drinking;

        if (dialogueData != null)
        {
            dialogueManager.StartDialogue(dialogueData.drinkDialogue);
        }
    }

    public void LeaveBar()
    {
        currentDState = NPCDialogueState.Leaving;

        if (dialogueData != null)
        {
            dialogueManager.StartDialogue(dialogueData.exitDialogue);
        }
    }
}
