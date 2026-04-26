using UnityEngine;

public enum IngredientOwner
{
    Player,
    Barman,
    Special
}

public enum IngredientCategory
{
    People,
    Career,
    Places,
    Objects,
    Special
}

[CreateAssetMenu(menuName = "Bartender/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    public IngredientOwner owner;
    public IngredientCategory category;
}