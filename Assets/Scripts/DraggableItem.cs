using System;
using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    private bool isDragging = false;

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
            Debug.Log("Dropped into glass!");
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

    internal void BeginDrag()
    {
        isDragging = true;
    }
}