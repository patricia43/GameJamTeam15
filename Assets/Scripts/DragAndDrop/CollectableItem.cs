using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public IngredientData ingredientData;

    private void OnMouseDown()
    {
        SpawnDraggableCopy();
    }

    void SpawnDraggableCopy()
{
    Vector3 mouseWorld = GetMouseWorldPosition();

    GameObject clone = Instantiate(gameObject, mouseWorld, Quaternion.identity);

    clone.GetComponent<CollectableItem>().enabled = false;

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
}