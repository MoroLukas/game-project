using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject PauseMenuUI;
    private bool isPaused = false;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;      // Disattiva VSync
        Application.targetFrameRate = 60;
        if (Instance != null) { Destroy(gameObject); return; }

        Instance = this;
    }

    void Start()
    {
        playerHealth.OnDeath += HandleGameOver;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
     }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            PauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            PauseMenuUI.SetActive(false);
        }
    }

    void OnDestroy()
    {
        playerHealth.OnDeath -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0f; // pausa il gioco
    }


    public void RestartRun()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeRun()
    {
        Time.timeScale = 1f;
        GameOverUI.SetActive(false);
        PauseMenuUI.SetActive(false);

    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}