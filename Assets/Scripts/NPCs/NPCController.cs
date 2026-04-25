using UnityEngine;
using System.Linq;

// FOR TESTING - NOT FINAL

public class NPCController : MonoBehaviour
{
    public NPCData data;

    public void TriggerDialogue(DialogueTrigger trigger)
    {
        var group = data.dialogueGroups
            .FirstOrDefault(g => g.trigger == trigger);

        if (group == null || group.sequences.Count == 0)
        {
            Debug.Log("No dialogue found for trigger: " + trigger);
            return;
        }

        var chosen = group.sequences[
            Random.Range(0, group.sequences.Count)
        ];

        Debug.Log("---- Dialogue Start (" + trigger + ") ----");

        foreach (var line in chosen.lines)
        {
            if (line.speaker == Speaker.NPC)
                Debug.Log(data.npcName + ": " + line.text);
            else
                Debug.Log("BARMAN: " + line.text);
        }

        Debug.Log("---- Dialogue End ----");
    }

    // Temporary test input
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            TriggerDialogue(DialogueTrigger.Order);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            TriggerDialogue(DialogueTrigger.Hit);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            TriggerDialogue(DialogueTrigger.Drop);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            TriggerDialogue(DialogueTrigger.Goodbye);
    }
}