using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    private Queue<DialogueLines> lines = new Queue<DialogueLines>();
    
    public void StartDialogue(DialogueLines[] dialogueLines)
    {
        lines.Clear();

        foreach (DialogueLines line in dialogueLines)
        {
            lines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLines line = lines.Dequeue();

        Debug.Log(line.NPCName + ": " + line.text);
    }

    private void EndDialogue()
    {
        Debug.Log("Dialgoue ended");
    }
}
