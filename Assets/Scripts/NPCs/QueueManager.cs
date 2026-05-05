using UnityEngine;
using System.Collections.Generic;

public class QueueManager : MonoBehaviour
{
    public List<NPCInteractionTest> npcQueue = new List<NPCInteractionTest>();
    public NPCInteractionTest currentNPCAtBar;

    [Header("Queue Positioning")]
    public Transform queuePoint;   // front of queue
    public float spacing = 1.5f;   // distance between NPCs

    // public NPCInteractionTest CurrentNPCAtBar { get; private set; }

    void Start()
    {
        foreach (var npc in npcQueue)
        {
            npc.OnServiceFinished += HandleServiceFinished;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Playing)
        {
            if (npcQueue.Count > 0 && currentNPCAtBar == null)
            {
                TakeOrder();
            }
        }
    }

    // ==========================
    // BUTTON METHODS
    // ==========================

    public void TakeOrder()
    {


        if (npcQueue.Count == 0)
            return;

        NPCInteractionTest npc = npcQueue[0];

        npcQueue.RemoveAt(0);
        npcQueue.Add(npc);

        currentNPCAtBar = npc;

        npc.TakeOrder();
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

        // Auto call next NPC if any are waiting
        if (npcQueue.Count > 0 && currentNPCAtBar == null)
        {
            TakeOrder();
        }
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