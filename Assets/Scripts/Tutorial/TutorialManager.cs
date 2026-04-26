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

    private bool waitingForIntroClick = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (waitingForIntroClick && Input.GetMouseButtonDown(0))
        {
            HideIntroMessage();
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

    void HideIntroMessage()
    {
        if (tutorialMessagePanel != null)
            tutorialMessagePanel.SetActive(false);

        waitingForIntroClick = false;

        GameManager.Instance.SetState(GameState.Tutorial);

        lemonHighlight.StartFlicker();
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
            mixButtonHighlight.StartFlicker();
            step = 4; // waiting for Mix click
        }
    }

    public void NotifyMixPressed()
    {
        if (GameManager.Instance.CurrentState != GameState.Tutorial)
            return;

        if (step == 4)
        {
            mixButtonHighlight.StopFlicker();
            step = 5;

            CompleteTutorial();
        }
    }

    void CompleteTutorial()
    {
        Debug.Log("Tutorial Complete!");

        if (takeOrderButton != null)
            takeOrderButton.SetActive(true);

        GameManager.Instance.SetState(GameState.Playing);
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