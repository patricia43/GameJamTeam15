using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CollectableItem : MonoBehaviour
{
    private void OnMouseDown()
    {
        SpawnDraggableCopy();
    }

    void SpawnDraggableCopy()
    {
        Vector3 mouseWorld = GetMouseWorldPosition();

        // Clone this object
        GameObject clone = Instantiate(gameObject, mouseWorld, Quaternion.identity);
        clone.GetComponent<DraggableItem>().BeginDrag();

        clone.GetComponent<CollectableItem>().enabled = false;
        clone.AddComponent<DraggableItem>();
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        return pos;
    }
}