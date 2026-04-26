using UnityEngine;
using TMPro;

public class IngredientHoverUI : MonoBehaviour
{
    public static IngredientHoverUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI text;

    private RectTransform rect;

    void Awake()
    {
        Instance = this;
        rect = panel.GetComponent<RectTransform>();
        panel.SetActive(false);
    }

    void Update()
    {
        if (panel.activeSelf)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect.parent as RectTransform,
                Input.mousePosition,
                null,
                out pos
            );

            rect.anchoredPosition = pos + new Vector2(0f, 30f);
        }
    }

    public void Show(string ingredientName, bool isDelirium)
    {
        text.text = ingredientName;
        text.color = isDelirium ? Color.red : Color.white;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}