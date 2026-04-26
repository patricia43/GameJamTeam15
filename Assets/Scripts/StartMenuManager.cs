using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Set dress");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}