using UnityEngine;
using System.Collections.Generic;

public class QueueManager : MonoBehaviour
{
    public List<NPCInteractionTest> npcQueue = new List<NPCInteractionTest>();
    private NPCInteractionTest currentNPCAtBar;

    [Header("Queue Positioning")]
    public Transform queuePoint;   // front of queue
    public float spacing = 1.5f;   // distance between NPCs

    void Start()
    {
        foreach (var npc in npcQueue)
        {
            npc.OnServiceFinished += HandleServiceFinished;
        }
    }

    // ==========================
    // BUTTON METHODS
    // ==========================

    public void TakeOrder()
    {
        if (npcQueue.Count == 0 || currentNPCAtBar != null)
            return;

        currentNPCAtBar = npcQueue[0];

        npcQueue.RemoveAt(0);
        npcQueue.Add(currentNPCAtBar);

        currentNPCAtBar.TakeOrder();

        UpdateQueuePositions();
        DebugQueueOrder();
    }

    public void ServeDrink()
    {
        if (currentNPCAtBar == null)
            return;

        currentNPCAtBar.ServeDrink();
    }

    public void ServeDrinkWithDelirium()
    {
        if (currentNPCAtBar == null)
            return;

        currentNPCAtBar.ServeDrinkWithDelirium();
    }

    // ==========================
    // QUEUE LOGIC
    // ==========================

    void HandleServiceFinished(NPCInteractionTest npc)
    {
        if (npc.currentState == NPCState.Dead)
        {
            npcQueue.Remove(npc);
            Debug.Log(npc.name + " removed from queue (dead).");
        }

        if (npc == currentNPCAtBar)
            currentNPCAtBar = null;

        UpdateQueuePositions();
        DebugQueueOrder();
    }

    void DebugQueueOrder()
    {
        string order = "Queue: ";
        foreach (var npc in npcQueue)
        {
            order += npc.name + " -> ";
        }
        Debug.Log(order);
    }

    void UpdateQueuePositions()
    {
        for (int i = 0; i < npcQueue.Count; i++)
        {
            Vector3 targetPos = queuePoint.position + Vector3.left * spacing * i;

            npcQueue[i].MoveToQueuePosition(targetPos);
        }
    }
}