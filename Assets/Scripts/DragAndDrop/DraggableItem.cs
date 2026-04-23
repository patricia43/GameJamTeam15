using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    private IngredientData ingredient;
    private bool isDragging = false;

    public void SetIngredient(IngredientData data)
    {
        ingredient = data;
    }

    void Update()
    {
        if (!isDragging) return;

        FollowMouse();

        if (Input.GetMouseButtonUp(0))
        {
            Release();
        }
    }

    void FollowMouse()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        transform.position = pos;
    }

    void Release()
    {
        isDragging = false;

        if (IsOverGlass())
        {
            if (ingredient == null)
            {
                Debug.LogError("NO INGREDIENT DATA");
            } else
            {
                GlassManager.Instance.AddIngredient(ingredient);
                // Debug.Log("Dropped: " + ingredient.ingredientName);
            }
        }

        Destroy(gameObject);
    }

    bool IsOverGlass()
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Glass"))
                return true;
        }

        return false;
    }

    public void BeginDrag()
    {
        isDragging = true;
    }
}