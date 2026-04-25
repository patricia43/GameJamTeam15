using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    private List<DialogueLines> lines = new List<DialogueLines>();
    private int currentIndex = 0;
    
    public void StartDialogue(DialogueLines[] dialogueLines)
    {
        lines.Clear();
        lines.AddRange(dialogueLines);
        currentIndex = 0;

        dialoguePanel.SetActive(true);

        DisplayNextLine();
    }

    void Update()
    {
        if (lines.Count > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextLine();
        }
    }

    public void DisplayCurrentLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLines line = lines[currentIndex];

        dialogueText.text = line.NPCName + ": " + line.text;

        //Debug.Log(line.NPCName + ": " + line.text);
    }

    public void DisplayNextLine()
    {
        currentIndex++;
        DisplayCurrentLine();
    }

    private void EndDialogue()
    {
        //Debug.Log("Dialgoue ended");
        dialoguePanel.SetActive(false);
        lines.Clear();
    }
}
