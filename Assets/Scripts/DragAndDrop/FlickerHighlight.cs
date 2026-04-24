using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class FlickerHighlight : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    private Coroutine flickerRoutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void StartFlicker()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        flickerRoutine = StartCoroutine(Flicker());
    }

    public void StopFlicker()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        sr.color = originalColor;
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            sr.color = Color.white * 1.5f;
            yield return new WaitForSeconds(0.25f);

            sr.color = originalColor;
            yield return new WaitForSeconds(0.25f);
        }
    }
}