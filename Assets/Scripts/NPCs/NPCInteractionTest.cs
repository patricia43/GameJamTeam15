using UnityEngine;
using System.Collections;

public enum ServiceState
{
    InQueue,
    AtBar,
    Leaving
}

public class NPCInteractionTest : MonoBehaviour
{
    [Header("Positions")]
    public Transform queuePoint;
    public Transform barPoint;
    public Transform exitPoint;

    [Header("State")]
    public NPCState currentState = NPCState.Normal;
    public ServiceState serviceState = ServiceState.InQueue;

    private int normalDrinkCount = 0;
    private int deliriumCount = 0;

    private SpriteRenderer sr;
    private bool isMoving = false;

    public System.Action<NPCInteractionTest> OnServiceFinished;

    private NPCController_ale npcDialogueController;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        npcDialogueController = GetComponent<NPCController_ale>();

        transform.position = queuePoint.position;
        UpdateVisual();
    }

    // ======================
    // BUTTON METHODS
    // ======================

    public void TakeOrder()
    {
        if (serviceState != ServiceState.InQueue || isMoving)
            return;

        Debug.Log("Taking order.");
        StartCoroutine(MoveTo(barPoint.position));
        serviceState = ServiceState.AtBar;

        // trigger enter dialogue
        npcDialogueController?.EnterBar();
    }

    public void ServeDrink()
    {
        if (serviceState != ServiceState.AtBar || currentState == NPCState.Dead)
            return;

        normalDrinkCount++;
        UpdateStateFromNormalDrinks();

        // trigger drinking dialogue
        npcDialogueController?.ReceiveDrink();

        AfterService();
    }

    public void ServeDrinkWithDelirium()
    {
        if (serviceState != ServiceState.AtBar || currentState == NPCState.Dead)
            return;

        deliriumCount++;
        UpdateStateFromDelirium();

        // trigger drinking dialogue
        npcDialogueController?.ReceiveDrink();

        AfterService();
    }

    void AfterService()
    {
        if (currentState == NPCState.Dead || currentState == NPCState.Insane)
        {
            StartCoroutine(MoveAndDie(exitPoint.position));
            serviceState = ServiceState.Leaving;
        }
        else
        {
            StartCoroutine(MoveBackToQueue());
        }
    }

    IEnumerator MoveAndDie(Vector3 target)
    {
        // trigger exit dialogue
        npcDialogueController?.LeaveBar();

        yield return StartCoroutine(MoveTo(target));

        SetState(NPCState.Dead);

        OnServiceFinished?.Invoke(this);
    }

    IEnumerator MoveBackToQueue()
    {
        yield return StartCoroutine(MoveTo(queuePoint.position));

        serviceState = ServiceState.InQueue;

        OnServiceFinished?.Invoke(this);
    }

    // ======================
    // MOVEMENT
    // ======================

    IEnumerator MoveTo(Vector3 target)
    {
        isMoving = true;

        float duration = 0.5f;
        float time = 0f;

        Vector3 start = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }

    public void MoveToQueuePosition(Vector3 position)
    {
        if (serviceState == ServiceState.InQueue)
        {
            StopAllCoroutines();
            StartCoroutine(MoveTo(position));
        }
    }

    // ======================
    // STATE LOGIC
    // ======================

    void UpdateStateFromNormalDrinks()
    {
        if (normalDrinkCount == 1)
            SetState(NPCState.Mild);
        else if (normalDrinkCount == 2)
            SetState(NPCState.Wild);
        else if (normalDrinkCount == 4)
            SetState(NPCState.Crazy);
        else if (normalDrinkCount >= 6)
            SetState(NPCState.Insane);
    }

    void UpdateStateFromDelirium()
    {
        if (deliriumCount == 1 || deliriumCount == 2)
        {
            if (currentState < NPCState.Wild)
                SetState(NPCState.Wild);
        }
        else if (deliriumCount == 3)
        {
            if (Random.value > 0.5f)
            {
                // dies
                StartCoroutine(MoveAndDie(exitPoint.position));
                serviceState = ServiceState.Leaving;
            } else
            {
                // also dies?
                SetState(NPCState.Insane);
            }
        }
        else if (deliriumCount >= 4)
        {
            SetState(NPCState.Insane);
        }
    }

    void SetState(NPCState newState)
    {
        currentState = newState;
        UpdateVisual();
        Debug.Log("NPC state: " + currentState);

        if (currentState == NPCState.Dead)
        {
            sr.color = Color.black;
        }
    }

    void UpdateVisual()
    {
        switch (currentState)
        {
            case NPCState.Normal:
                sr.color = Color.gray; break;
            case NPCState.Mild:
                sr.color = Color.green; break;
            case NPCState.Wild:
                sr.color = Color.yellow; break;
            case NPCState.Crazy:
                sr.color = new Color(1f, 0.5f, 0f); break;
            case NPCState.Insane:
                sr.color = Color.red; break;
            case NPCState.Dead:
                sr.color = Color.black; break;
        }
    }
}