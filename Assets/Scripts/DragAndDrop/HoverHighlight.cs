using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HoverHighlight : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = Color.white * 1.5f;  // white highlight
    }

    private void OnMouseExit()
    {
        sr.color = originalColor;
    }
}