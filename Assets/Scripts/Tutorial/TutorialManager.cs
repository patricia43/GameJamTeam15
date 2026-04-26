using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public IngredientData lemonIngredient;
    public IngredientData waterIngredient;

    public FlickerHighlight lemonHighlight;
    public FlickerHighlight waterHighlight;
    public FlickerHighlight glassHighlight;

    private int step = 0;

    public FlickerHighlight mixButtonHighlight;

    [Header("Tutorial UI")]
    public GameObject tutorialMessagePanel;
    public TextMeshProUGUI tutorialMessageText;

    [Header("Buttons")]
    public GameObject takeOrderButton;

    [Header("Button Text Flicker")]
    public FlickerText mixButtonTextFlicker;
    public FlickerText takeOrderTextFlicker;

    private bool waitingForIntroClick = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!waitingForIntroClick || !Input.GetMouseButtonDown(0))
            return;

        tutorialMessagePanel.SetActive(false);
        waitingForIntroClick = false;

        // INTRO CLICK
        if (step == 0)
        {
            ShowInstructionMessage("Drag and drop the lemon into the glass,\nthen the water,\nthen click 'Mix & Serve'.");
            step = -1; // temporary instruction step
            return;
        }

        // INSTRUCTION CLICK
        if (step == -1)
        {
            GameManager.Instance.SetState(GameState.Tutorial);
            lemonHighlight.StartFlicker();
            step = 0;
            return;
        }

        // COMPLETION CLICK
        if (step == 5)
        {
            if (takeOrderButton != null)
                takeOrderButton.SetActive(true);

            takeOrderTextFlicker?.StartFlicker();

            GameManager.Instance.SetState(GameState.Playing);
            step = 6;
        }
    }

    public void BeginTutorial()
    {
        step = 0;

        if (takeOrderButton != null)
            takeOrderButton.SetActive(false);

        ShowIntroMessage("Let's mix and serve a Lemonade to start!");
    }

    void ShowIntroMessage(string message)
    {
        if (tutorialMessageText != null)
            tutorialMessageText.text = message;

        if (tutorialMessagePanel != null)
            tutorialMessagePanel.SetActive(true);

        waitingForIntroClick = true;

        GameManager.Instance.SetState(GameState.Dialogue);
    }

    //void HideIntroMessage()
    //{
    //    if (tutorialMessagePanel != null)
    //        tutorialMessagePanel.SetActive(false);

    //    waitingForIntroClick = false;

    //    GameManager.Instance.SetState(GameState.Tutorial);

    //    lemonHighlight.StartFlicker();
    //}

    void HideIntroMessage()
    {
        tutorialMessagePanel.SetActive(false);
        waitingForIntroClick = false;

        ShowInstructionMessage("Drag and drop the lemon into the glass,\nthen water,\nthen click 'Mix & Serve'.");
    }

    void ShowInstructionMessage(string message)
    {
        tutorialMessageText.text = message;
        tutorialMessagePanel.SetActive(true);
        waitingForIntroClick = true;

        GameManager.Instance.SetState(GameState.Dialogue);
    }

    public void NotifyIngredientPicked(IngredientData ingredient)
    {
        if (step == 0 && ingredient == lemonIngredient)
        {
            lemonHighlight.StopFlicker();
            glassHighlight.StartFlicker();
            step = 1;
        }
        else if (step == 2 && ingredient == waterIngredient)
        {
            waterHighlight.StopFlicker();
            glassHighlight.StartFlicker();
            step = 3;
        }
    }

    public void NotifyIngredientDropped(IngredientData ingredient)
    {
        if (step == 1 && ingredient == lemonIngredient)
        {
            glassHighlight.StopFlicker();
            waterHighlight.StartFlicker();
            step = 2;
        }
        else if (step == 3 && ingredient == waterIngredient)
        {
            glassHighlight.StopFlicker();
            mixButtonTextFlicker?.StartFlicker();
            step = 4; // waiting for Mix click
        }
    }

    public void NotifyMixPressed()
    {
        if (GameManager.Instance.CurrentState != GameState.Tutorial)
            return;

        if (step == 4)
        {
            mixButtonTextFlicker?.StopFlicker();
            step = 5;

            CompleteTutorial();
        }
    }

    void CompleteTutorial()
    {
        ShowCompletionMessage("Now I am ready to take orders.");
    }

    void ShowCompletionMessage(string message)
    {
        tutorialMessageText.text = message;
        tutorialMessagePanel.SetActive(true);
        waitingForIntroClick = true;

        GameManager.Instance.SetState(GameState.Dialogue);
    }

    public bool CanInteractWith(IngredientData ingredient)
    {
        if (GameManager.Instance.CurrentState != GameState.Tutorial)
            return true;

        if (step == 0 && ingredient == lemonIngredient)
            return true;

        if (step == 2 && ingredient == waterIngredient)
            return true;

        return false;
    }
}