using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public IngredientData ingredientData;

    //private void OnMouseDown()
    //{
    //    if (GameManager.Instance.IsGameplayBlocked())
    //    {
    //        if (!TutorialManager.Instance.CanInteractWith(ingredientData))
    //            return;
    //    }

    //    SpawnDraggableCopy();
    //}

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsGameplayBlocked())
        {
            if (!TutorialManager.Instance.CanInteractWith(ingredientData))
                return;
        }

        // Hide hover immediately when clicking
        IngredientHoverUI.Instance?.Hide();

        SpawnDraggableCopy();
    }

    void SpawnDraggableCopy()
    {
        Vector3 mouseWorld = GetMouseWorldPosition();

        GameObject clone = Instantiate(gameObject, mouseWorld, Quaternion.identity);

        clone.GetComponent<CollectableItem>().enabled = false;

        Collider2D col = clone.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        DraggableItem drag = clone.GetComponent<DraggableItem>();

        drag.SetIngredient(ingredientData);
        drag.BeginDrag();
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        return pos;
    }

    private void OnMouseEnter()
    {
        bool isDelirium = ingredientData.owner == IngredientOwner.Special &&
                          ingredientData.category == IngredientCategory.Special;

        IngredientHoverUI.Instance?.Show(
            ingredientData.ingredientName,
            isDelirium
        );
    }

    private void OnMouseExit()
    {
        IngredientHoverUI.Instance?.Hide();
    }

    bool IsMouseOver()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        return hit != null && hit.gameObject == gameObject;
    }
}