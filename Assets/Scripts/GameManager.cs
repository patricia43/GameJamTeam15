using UnityEngine;
using System;

public enum GameState
{
    Playing,
    Paused,
    Tutorial,
    GameOver
}

public class GameManager : MonoBehaviour
{
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
        SetState(GameState.Playing);

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing)
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
                AudioListener.pause = false;
                if (pausePanel != null)
                    pausePanel.SetActive(false);
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                if (pausePanel != null)
                    pausePanel.SetActive(true);
                break;

            case GameState.Tutorial:
                Time.timeScale = 1f;
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
               CurrentState == GameState.GameOver;
    }
}