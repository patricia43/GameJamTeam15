using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Bartender/Recipe Database")]
public class RecipeDatabase : ScriptableObject
{
    public List<CocktailRecipe> allRecipes;

    public CocktailRecipe FindMatch(List<IngredientData> selected)
    {
        foreach (var recipe in allRecipes)
        {
            if (IngredientsMatch(recipe.requiredIngredients, selected))
                return recipe;
        }

        return null;
    }

    private bool IngredientsMatch(List<IngredientData> recipe, List<IngredientData> selected)
    {
        if (recipe.Count != selected.Count)
            return false;

        return !recipe.Except(selected).Any() &&
               !selected.Except(recipe).Any();
    }
}