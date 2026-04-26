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

        // transform.position = queuePoint.position;
        UpdateVisual();
    }

    // ======================
    // BUTTON METHODS
    // ======================

    public void TakeOrder()
    {
        if (serviceState != ServiceState.InQueue || isMoving)
            return;

        StartCoroutine(TakeOrderSequence());
    }

    IEnumerator TakeOrderSequence()
    {
        serviceState = ServiceState.AtBar;

        yield return StartCoroutine(MoveTo(barPoint.position));

        npcDialogueController?.EnterBar();

        yield return new WaitUntil(() => GameManager.Instance.CurrentState != GameState.Dialogue);
    }

    public void ServeDrink()
    {
        if (serviceState != ServiceState.AtBar || currentState == NPCState.Dead)
            return;

        //if (currentState == NPCState.Insane)
        //{
        //    StartCoroutine(InsaneServeSequence());
        //}
        //else
        //{
        //    StartCoroutine(ServeDrinkSequence(false));
        //}
        StartCoroutine(ServeDrinkSequence(false));
    }

    public void ServeDrinkWithDelirium()
    {
        if (serviceState != ServiceState.AtBar || currentState == NPCState.Dead)
            return;

        if (currentState == NPCState.Insane)
        {
            StartCoroutine(InsaneServeSequence());
        }
        else
        {
            StartCoroutine(ServeDrinkSequence(true));
        }
    }

    IEnumerator InsaneServeSequence()
    {
        serviceState = ServiceState.Leaving;

        // Say goodbye
        npcDialogueController?.LeaveBar();
        yield return new WaitUntil(() => GameManager.Instance.CurrentState != GameState.Dialogue);

        // Move right
        yield return StartCoroutine(MoveTo(exitPoint.position));

        SetState(NPCState.Dead);
        OnServiceFinished?.Invoke(this);
    }

    IEnumerator ServeDrinkSequence(bool delirium)
    {
        if (delirium)
        {
            deliriumCount++;
            UpdateStateFromDelirium();
        }
        else
        {
            normalDrinkCount++;
            UpdateStateFromNormalDrinks();
        }

        if (currentState == NPCState.Insane)
        {
            yield return StartCoroutine(InsaneServeSequence());
            yield break;
        }

        // Drinking dialogue
        npcDialogueController?.ReceiveDrink();
        yield return new WaitUntil(() => GameManager.Instance.CurrentState != GameState.Dialogue);

        // Animation delay
        yield return new WaitForSeconds(0.5f);

        // Drop dialogue
        npcDialogueController?.DropDrink();
        yield return new WaitUntil(() => GameManager.Instance.CurrentState != GameState.Dialogue);

        // Move normally
        yield return StartCoroutine(AfterServiceSequence());
    }

    IEnumerator AfterServiceSequence()
    {
        if (currentState == NPCState.Dead || currentState == NPCState.Insane)
        {
            serviceState = ServiceState.Leaving;

            // Exit dialogue BEFORE moving
            npcDialogueController?.LeaveBar();
            yield return new WaitUntil(() => GameManager.Instance.CurrentState != GameState.Dialogue);

            yield return StartCoroutine(MoveTo(exitPoint.position));

            SetState(NPCState.Dead);
            OnServiceFinished?.Invoke(this);
        }
        else
        {
            yield return StartCoroutine(MoveBackToQueue());
        }
    }

    //void AfterService()
    //{
    //    if (currentState == NPCState.Dead || currentState == NPCState.Insane)
    //    {
    //        StartCoroutine(MoveAndDie(exitPoint.position));
    //        serviceState = ServiceState.Leaving;
    //    }
    //    else
    //    {
    //        StartCoroutine(MoveBackToQueue());
    //    }
    //}

    //IEnumerator MoveAndDie(Vector3 target)
    //{
    //    // trigger exit dialogue
    //    npcDialogueController?.LeaveBar();

    //    yield return StartCoroutine(MoveTo(target));

    //    SetState(NPCState.Dead);

    //    OnServiceFinished?.Invoke(this);
    //}

    IEnumerator MoveBackToQueue()
    {
        yield return StartCoroutine(MoveTo(queuePoint.position));

        serviceState = ServiceState.InQueue;

        npcDialogueController?.AdvanceDialogueSet();

        OnServiceFinished?.Invoke(this);
    }

    // ======================
    // MOVEMENT
    // ======================

    IEnumerator MoveTo(Vector3 target)
    {
        // Wait until dialogue finishes
        //while (GameManager.Instance.CurrentState == GameState.Dialogue)
        //    yield return null;

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
        else if (normalDrinkCount == 3)
            SetState(NPCState.Crazy);
        else if (normalDrinkCount >= 4)
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
                StartCoroutine(InstantDeathSequence());
            }
            else
            {
                SetState(NPCState.Insane);
            }
        }
        else if (deliriumCount >= 4)
        {
            SetState(NPCState.Insane);
        }
    }

    IEnumerator InstantDeathSequence()
    {
        serviceState = ServiceState.Leaving;

        npcDialogueController?.LeaveBar();
        yield return new WaitUntil(() => GameManager.Instance.CurrentState != GameState.Dialogue);

        yield return StartCoroutine(MoveTo(exitPoint.position));

        SetState(NPCState.Dead);
        OnServiceFinished?.Invoke(this);
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