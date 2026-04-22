using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Bartender/Cocktail Recipe")]
public class CocktailRecipe : ScriptableObject
{
    public string cocktailName;

    public List<RecipeSlot> slots;
}

[System.Serializable]
public class RecipeSlot
{
    public bool isFixedIngredient;

    public IngredientData fixedIngredient;

    public IngredientOwner requiredOwner;
    public IngredientCategory requiredCategory;
}