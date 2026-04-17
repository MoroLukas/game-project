using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject PauseMenuUI;

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
        //Ricarica la scena attuale per reettare tutto
        //SceneManager.LoadScene("TestLukas");
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
        SceneManager.LoadScene("TestLukas");
    }
}