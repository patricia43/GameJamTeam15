using UnityEngine;
using System.Collections.Generic;

public class GlassManager : MonoBehaviour
{
    public static GlassManager Instance;

    public RecipeDatabase recipeDatabase;

    private List<IngredientData> currentIngredients = new List<IngredientData>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddIngredient(IngredientData ingredient)
    {
        if (ingredient == null)
        {
            Debug.LogError("Tried to add NULL ingredient!");
            return;
        }
        
        currentIngredients.Add(ingredient);
        DebugCurrentIngredients();
    }

    public void ResetGlass()
    {
        currentIngredients.Clear();
        Debug.Log("Glass reset.");
    }

    public void Mix()
    {
        var result = recipeDatabase.FindMatch(currentIngredients);

        if (result != null)
        {
            Debug.Log("Created cocktail: " + result.cocktailName);
        }
        else
        {
            Debug.Log("Invalid cocktail!");
        }

        currentIngredients.Clear();
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