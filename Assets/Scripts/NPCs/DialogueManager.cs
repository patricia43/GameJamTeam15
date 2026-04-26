using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    private List<DialogueLines> lines = new List<DialogueLines>();
    private int currentIndex = 0;

    public System.Action OnDialogueFinished;

    public void StartDialogue(DialogueLines[] dialogueLines)
    {
        lines.Clear();
        lines.AddRange(dialogueLines);
        currentIndex = 0;

        dialoguePanel.SetActive(true);

        // DisplayNextLine();
        DisplayCurrentLine();

        GameManager.Instance.SetState(GameState.Dialogue);
    }

    void Update()
    {
        if (lines.Count > 0 && Input.GetMouseButtonDown(0) 
            // && !EventSystem.current.IsPointerOverGameObject()
            )
        {
            DisplayNextLine();
        }
    }

    public void DisplayCurrentLine()
    {
        if (lines.Count == 0 || currentIndex < 0 || currentIndex >= lines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLines line = lines[currentIndex];

        if (!string.IsNullOrEmpty(line.NPCName) && line.NPCName.ToLower() == "barman")
            {
                dialogueText.text = line.NPCName + ": " + line.text;
            }
            else
            {
                dialogueText.text = line.text;
            }

        //Debug.Log(line.NPCName + ": " + line.text);
    }

    public void DisplayNextLine()
    {
        currentIndex++;
        DisplayCurrentLine();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        lines.Clear();

        GameManager.Instance.SetState(GameState.Playing);

        OnDialogueFinished?.Invoke();
        OnDialogueFinished = null;
    }
}
