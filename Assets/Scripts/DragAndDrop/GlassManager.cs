using UnityEngine;
using System.Collections.Generic;

public class GlassManager : MonoBehaviour
{
    public static GlassManager Instance;

    public RecipeDatabase recipeDatabase;

    private List<IngredientData> currentIngredients = new List<IngredientData>();

    public TMPro.TextMeshProUGUI resultText;
    public GameObject resultPanel;

    private bool waitingForResultClick = false;

    public QueueManager queueManager;

    private bool pendingServeWithDelirium = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        //Debug.Log("State: " + GameManager.Instance.CurrentState +
        //  " DialogueBlock: " + GameManager.Instance.IsDialogueActive);

        if (waitingForResultClick && Input.GetMouseButtonDown(0))
        {
            HideResultUI();
        }
        else if (!waitingForResultClick && Input.GetMouseButtonDown(0))
        {
            // Check if player clicked the glass
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Glass"))
                {
                    MixAndServe();
                    break;
                }
            }
        }
    }

    public void AddIngredient(IngredientData ingredient)
    {
        if (ingredient == null)
        {
            Debug.LogError("Tried to add NULL ingredient!");
            return;
        }

        // Enforce Cap: Max 2 Normal, Max 1 Delirium (Skip during Tutorial)
        if (GameManager.Instance.CurrentState != GameState.Tutorial)
        {
            int normalCount = 0;
            bool hasDelirium = false;

            foreach (var ing in currentIngredients)
            {
                if (ing.owner == IngredientOwner.Special && ing.category == IngredientCategory.Special)
                    hasDelirium = true;
                else
                    normalCount++;
            }

            bool isDelirium = (ingredient.owner == IngredientOwner.Special && ingredient.category == IngredientCategory.Special);

            if (isDelirium && hasDelirium)
            {
                Debug.Log("Cannot add another Delirium ingredient. Cap reached.");
                return;
            }
            
            if (!isDelirium && normalCount >= 2)
            {
                Debug.Log("Cannot add another Normal ingredient. Cap of 2 reached.");
                return;
            }
        }
        
        currentIngredients.Add(ingredient);
        DebugCurrentIngredients();

        // ANTIGRAVITY: Trigger the Bartender UI prompt when 2 ingredients are added
        if (GameManager.Instance.CurrentState != GameState.Tutorial)
        {
            int normalCount = 0;
            bool hasDelirium = false;
            foreach (var ing in currentIngredients)
            {
                if (ing.owner == IngredientOwner.Special && ing.category == IngredientCategory.Special)
                    hasDelirium = true;
                else
                    normalCount++;
            }

            // Only show the prompt once the player has placed exactly 2 normal ingredients
            if (normalCount == 2)
            {
                if (BartenderPromptUI.Instance != null)
                {
                    BartenderPromptUI.Instance.ShowPrompt(hasDelirium);
                }
            }
        }
    }

    public void EmptyGlass()
    {
        currentIngredients.Clear();

        // ANTIGRAVITY: Hide the prompt if the player dumps the glass
        if (BartenderPromptUI.Instance != null)
        {
            BartenderPromptUI.Instance.HidePrompt();
        }
        Debug.Log("Glass reset.");
    }

    public void MixAndServe()
    {
        if (currentIngredients.Count == 0)
            return;

        // ANTIGRAVITY: Hide the prompt since the player is serving the drink
        if (BartenderPromptUI.Instance != null)
        {
            BartenderPromptUI.Instance.HidePrompt();
        }

        TutorialManager.Instance?.NotifyMixPressed();

        bool containsDelirium = false;
        foreach (var ing in currentIngredients)
        {
            if (ing.owner == IngredientOwner.Special && ing.category == IngredientCategory.Special)
            {
                containsDelirium = true;
            }
        }

        List<IngredientData> filteredIngredients = new List<IngredientData>();

        // ANTIGRAVITY: Determine exactly how many ingredients the player needs to add.
        int requiredPlayerIngredients = containsDelirium ? 3 : 2;

        // ANTIGRAVITY: Enforce the rule that the player must have filled the glass completely before mixing.
        // We bypass this during the Tutorial to allow for the upcoming 3-ingredient Lemonade task.
        if (GameManager.Instance.CurrentState != GameState.Tutorial)
        {
            if (currentIngredients.Count != requiredPlayerIngredients)
            {
                Debug.Log($"Glass not full! Need {requiredPlayerIngredients} ingredients to mix, but have {currentIngredients.Count}.");
                return;
            }
        }

        foreach (var ing in currentIngredients)
        {
            filteredIngredients.Add(ing);
        }

        // Automatically add Barman's placeholder ingredient if NOT in tutorial
        if (GameManager.Instance.CurrentState != GameState.Tutorial)
        {
            int barmanIngredientNr = Random.Range(1, 100);
            IngredientData placeholderBarmanIngredient = ScriptableObject.CreateInstance<IngredientData>();
            placeholderBarmanIngredient.ingredientName = "Barman_Ingredient_" + barmanIngredientNr;
            placeholderBarmanIngredient.owner = IngredientOwner.Barman;
            // Setting a default category, this can be expanded if specific categories are needed to match recipes.
            placeholderBarmanIngredient.category = IngredientCategory.People; 
            
            filteredIngredients.Add(placeholderBarmanIngredient);
            currentIngredients.Add(placeholderBarmanIngredient);
        }

        var result = recipeDatabase.FindMatch(filteredIngredients);

        string ingredientList = "";

        for (int i = 0; i < currentIngredients.Count; i++)
        {
            ingredientList += currentIngredients[i].ingredientName;
            if (i < currentIngredients.Count - 1)
                ingredientList += ", ";
        }

        if (result != null)
        {
            ShowResultUI("You made: " + result.cocktailName +
                         "\nIngredients: " + ingredientList);
        }
        else
        {
            ShowResultUI("Brand new cocktail recipe!\nIngredients: " + ingredientList);
        }

        currentIngredients.Clear();

        // Store what type of serve should happen AFTER UI closes
        pendingServeWithDelirium = containsDelirium;
    }

    void ShowResultUI(string message)
    {
        if (resultText != null)
            resultText.text = message;

        if (resultPanel != null)
            resultPanel.SetActive(true);

        waitingForResultClick = true;

        // Optional: block gameplay while result is shown
        // GameManager.Instance.SetState(GameState.Dialogue);
        GameManager.Instance.StartDialogueBlock();
    }

    void HideResultUI()
    {
        Debug.Log("HideResultUI called. Current state: " + GameManager.Instance.CurrentState);

        if (resultPanel != null)
            resultPanel.SetActive(false);

        waitingForResultClick = false;

        GameManager.Instance.EndDialogueBlock();

        // If tutorial is active, complete it
        if (GameManager.Instance.CurrentState == GameState.Tutorial)
        {
            Debug.Log("Tutorial Complete!");
            GameManager.Instance.SetState(GameState.Playing);
        }

        ServeAfterMix();
    }

    void ServeAfterMix()
    {
        if (queueManager == null || queueManager.currentNPCAtBar == null)
            return;

        if (pendingServeWithDelirium)
            queueManager.currentNPCAtBar.ServeDrinkWithDelirium();
        else
            queueManager.currentNPCAtBar.ServeDrink();

        pendingServeWithDelirium = false;
    }

    void DebugCurrentIngredients()
    {
        string log = "Current ingredients: ";

        foreach (var ing in currentIngredients)
        {
            log += ing.ingredientName + ", ";
        }

        Debug.Log(log);
    }
}