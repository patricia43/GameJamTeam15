using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FlickerText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Coroutine flickerRoutine;

    [Header("Flicker Settings")]
    [Range(0f, 1f)] public float minAlpha = 0.2f;     // how transparent it gets
    public float fadeSpeed = 6f;                      // fade speed
    public float visibleHoldTime = 0.8f;              // how long it stays fully visible
    public float fadedHoldTime = 0.15f;               // how long it stays faded

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void StartFlicker()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        flickerRoutine = StartCoroutine(FlickerRoutine());
    }

    public void StopFlicker()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        SetAlpha(1f);
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // Fade out
            yield return FadeTo(minAlpha);

            yield return new WaitForSeconds(fadedHoldTime);

            // Fade back in
            yield return FadeTo(1f);

            yield return new WaitForSeconds(visibleHoldTime);
        }
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = text.alpha;
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * fadeSpeed;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time);
            SetAlpha(newAlpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    void SetAlpha(float value)
    {
        Color c = text.color;
        c.a = value;
        text.color = c;
    }
}