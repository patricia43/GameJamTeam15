using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class FlickerImage : MonoBehaviour
{
    private Image image;
    private Coroutine flickerRoutine;
    private Color originalColor;

    void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
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

        image.color = originalColor;
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // Fade out quickly
            yield return StartCoroutine(FadeTo(0.2f, 0.15f));

            // Stay slightly transparent longer
            yield return new WaitForSeconds(0.1f);

            // Fade back in smoothly
            yield return StartCoroutine(FadeTo(1f, 0.25f));

            // Stay visible longer
            yield return new WaitForSeconds(0.4f);
        }
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float time = 0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            time += Time.unscaledDeltaTime; // important for pause menus
            yield return null;
        }

        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
    }
}