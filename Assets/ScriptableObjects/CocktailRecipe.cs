using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Bartender/Cocktail Recipe")]
public class CocktailRecipe : ScriptableObject
{
    public string cocktailName;

    [Tooltip("Exact ingredients required")]
    public List<IngredientData> requiredIngredients;
}