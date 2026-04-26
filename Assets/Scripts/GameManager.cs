using UnityEngine;
using System;

public enum GameState
{
    Cutscene,
    Tutorial,
    Playing,
    Dialogue,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{

    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private float cutsceneDuration = 0.1f;

    public static GameManager Instance;

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    [Header("Pause Settings")]
    [SerializeField] private GameObject pausePanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(GameStartFlow());
    }

    private System.Collections.IEnumerator GameStartFlow()
    {
        SetState(GameState.Cutscene);

        yield return new WaitForSeconds(cutsceneDuration);

        SetState(GameState.Tutorial);

        tutorialManager.BeginTutorial();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState != GameState.Paused)
                PauseGame();
            else if (CurrentState == GameState.Paused)
                ResumeGame();
        }
    }

    // ==========================
    // STATE CONTROL
    // ==========================

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        HandleStateChange(newState);

        OnGameStateChanged?.Invoke(newState);

        // Debug.Log("Game State changed to: " + newState);
    }

    private void HandleStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                if (pausePanel != null)
                    pausePanel.SetActive(false);
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                if (pausePanel != null)
                    pausePanel.SetActive(true);
                break;

            case GameState.Cutscene:
                Time.timeScale = 1f; // animations still run
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case GameState.Tutorial:
                Time.timeScale = 1f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }
    }

    // ==========================
    // PUBLIC METHODS
    // ==========================

    public void PauseGame()
    {
        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        SetState(GameState.Playing);
    }

    public bool IsPaused()
    {
        return CurrentState == GameState.Paused;
    }

    public bool IsGameplayBlocked()
    {
        return CurrentState == GameState.Paused ||
               CurrentState == GameState.GameOver ||
               CurrentState == GameState.Cutscene ||
               CurrentState == GameState.Dialogue;
    }
}