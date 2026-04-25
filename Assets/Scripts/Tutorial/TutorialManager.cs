using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public IngredientData lemonIngredient;
    public IngredientData waterIngredient;

    public FlickerHighlight lemonHighlight;
    public FlickerHighlight waterHighlight;
    public FlickerHighlight glassHighlight;

    private int step = 0;

    void Awake()
    {
        Instance = this;
    }

    public void BeginTutorial()
    {
        step = 0;
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
            step = 4;
            Debug.Log("Tutorial Complete!");

            GameManager.Instance.SetState(GameState.Playing);
        }
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