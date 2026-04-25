using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    private IngredientData ingredient;
    private bool isDragging = false;
    public Renderer Liquid_renderer;
    public float sensitivity;
    public float smoothing;

    private Vector3 Last_position;
    private float Current_tilt;

    public void SetIngredient(IngredientData data)
    {
        ingredient = data;
    }

    void Update()
    {
        if (GameManager.Instance.IsGameplayBlocked())
            return;

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
        
        Vector3 displacement = transform.position - Last_position;
        float speed = displacement.magnitude/Time.deltaTime;
       
        Vector3 direction = displacement.normalized;
        float tilt_angle = Vector3.Dot(displacement, transform.right)/Time.deltaTime;
        float target_tilt = Mathf.Clamp(tilt_angle*sensitivity,-1,1);
        Current_tilt = Mathf.Lerp(Current_tilt, target_tilt, Time.deltaTime * smoothing);
        
        if (Liquid_renderer != null)
        {
            Liquid_renderer.material.SetFloat("_Tilt", Current_tilt); 
        }

        Last_position = transform.position;

        print(displacement);
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
                TutorialManager.Instance?.NotifyIngredientDropped(ingredient);
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

        TutorialManager.Instance?.NotifyIngredientPicked(ingredient);
    }
}