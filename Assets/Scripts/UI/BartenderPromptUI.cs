using UnityEngine;
using TMPro;

public class BartenderPromptUI : MonoBehaviour
{
    public static BartenderPromptUI Instance;

    [Header("UI References")]
    public GameObject promptPanel;
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI buttonText;

    private GlassManager glassManager;

    void Awake()
    {
        // ANTIGRAVITY: Singleton pattern to easily access this from GlassManager
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        glassManager = FindObjectOfType<GlassManager>();
        
        // ANTIGRAVITY: Start hidden
        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    // ANTIGRAVITY: Called by GlassManager when 2 or 3 ingredients are in the glass
    public void ShowPrompt(bool hasDelirium)
    {
        if (promptPanel != null)
            promptPanel.SetActive(true);

        if (hasDelirium)
        {
            if (promptText != null) promptText.text = "Ready to serve?";
            if (buttonText != null) buttonText.text = "Serve";
        }
        else
        {
            if (promptText != null) promptText.text = "Should I mix some delirium?";
            if (buttonText != null) buttonText.text = "...or maybe not";
        }
    }

    // ANTIGRAVITY: Called by GlassManager when mixing or emptying the glass
    public void HidePrompt()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    // ANTIGRAVITY: Hook this method up to the Button's OnClick event in the Unity Inspector
    public void OnServeButtonPressed()
    {
        HidePrompt();
        if (glassManager != null)
        {
            glassManager.MixAndServe();
        }
    }
}
