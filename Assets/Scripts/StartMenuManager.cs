using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;

    private bool isSettingsOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC pressed - toggling settings.");
            ToggleSettings();
        }
    }

    public void StartGame()
    {
        Debug.Log("Start Game button pressed.");
        SceneManager.LoadScene("Set dress_cu animatii");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game button pressed.");
        Application.Quit();
    }

    public void ToggleSettings()
    {
        if (settingsPanel == null)
        {
            Debug.LogWarning("Settings panel is NULL!");
            return;
        }

        isSettingsOpen = !isSettingsOpen;
        settingsPanel.SetActive(isSettingsOpen);

        Debug.Log("Settings toggled. Now open: " + isSettingsOpen);
    }

    public void OpenSettings()
    {
        if (settingsPanel == null)
        {
            Debug.LogWarning("Settings panel is NULL!");
            return;
        }

        isSettingsOpen = true;
        settingsPanel.SetActive(true);

        Debug.Log("Settings opened.");
    }

    public void CloseSettings()
    {
        if (settingsPanel == null)
        {
            Debug.LogWarning("Settings panel is NULL!");
            return;
        }

        isSettingsOpen = false;
        settingsPanel.SetActive(false);

        Debug.Log("Settings closed.");
    }
}