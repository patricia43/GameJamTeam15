using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Bartender/Recipe Database")]
public class RecipeDatabase : ScriptableObject
{
    public List<CocktailRecipe> allTemplates;

    public CocktailRecipe FindMatch(List<IngredientData> selected)
    {
        foreach (var template in allTemplates)
        {
            if (MatchesTemplate(template, selected))
                return template;
        }

        return null;
    }

    private bool MatchesTemplate(CocktailRecipe template, List<IngredientData> selected)
    {
        if (selected.Count != template.slots.Count)
            return false;

        List<IngredientData> remaining = new List<IngredientData>(selected);

        foreach (var slot in template.slots)
        {
            bool matched = false;

            for (int i = 0; i < remaining.Count; i++)
            {
                var ing = remaining[i];

                if (slot.isFixedIngredient)
                {
                    if (ing == slot.fixedIngredient)
                    {
                        matched = true;
                        remaining.RemoveAt(i);
                        break;
                    }
                }
                else
                {
                    if (ing.owner == slot.requiredOwner &&
                        ing.category == slot.requiredCategory)
                    {
                        matched = true;
                        remaining.RemoveAt(i);
                        break;
                    }
                }
            }

            if (!matched)
                return false;
        }

        return true;
    }
}