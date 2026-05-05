using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //public TextMeshProUGUI dialogueText;
    //public GameObject dialoguePanel;

    [Header("NPC Dialogue UI")]
    public GameObject npcPanel;
    public TextMeshProUGUI npcText;

    [Header("Barman Dialogue UI")]
    public GameObject barmanPanel;
    public TextMeshProUGUI barmanText;

    private List<DialogueLines> lines = new List<DialogueLines>();
    private int currentIndex = 0;
    private bool canAdvance = false;

    public System.Action OnDialogueFinished;

    public void StartDialogue(DialogueLines[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        lines.Clear();
        lines.AddRange(dialogueLines);

        currentIndex = 0;

        // Make sure both panels start hidden
        npcPanel.SetActive(false);
        barmanPanel.SetActive(false);

        GameManager.Instance.SetState(GameState.Dialogue);

        DisplayCurrentLine();

        canAdvance = false;
        StartCoroutine(EnableAdvanceDelay());
    }

    private System.Collections.IEnumerator EnableAdvanceDelay()
    {
        yield return null; // Wait one frame to avoid same-frame click registration
        canAdvance = true;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Dialogue)
            return;

        if (GameManager.Instance.IsMenuOpen())
            return;

        if (canAdvance && lines.Count > 0 && Input.GetMouseButtonDown(0))
        {
            canAdvance = false;
            DisplayNextLine();
            StartCoroutine(EnableAdvanceDelay());
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

        bool isBarman = line.NPCName.ToLower() == "barman";

        if (isBarman)
        {
            // Activate barman UI
            barmanPanel.SetActive(true);
            npcPanel.SetActive(false);

            barmanText.text = line.text;
        }
        else
        {
            // Activate npc UI
            npcPanel.SetActive(true);
            barmanPanel.SetActive(false);

            npcText.text = line.text;
        }
    }

    public void DisplayNextLine()
    {
        currentIndex++;
        DisplayCurrentLine();
    }

    private void EndDialogue()
    {
        npcPanel.SetActive(false);
        barmanPanel.SetActive(false);

        lines.Clear();

        GameManager.Instance.SetState(GameState.Playing);

        OnDialogueFinished?.Invoke();
    }
}
