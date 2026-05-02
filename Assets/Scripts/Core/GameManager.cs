using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Scenes (in order)")]
    public string[] levelScenes = { "Level1", "Level2", "Level3" };

    private static int persistedScore = 0;
    private static int persistedLevelIndex = 0;

    private int score;
    private int currentLevelIndex;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        score = persistedScore;
        currentLevelIndex = persistedLevelIndex;
    }

    private bool isGameOver = false;
    private bool isPaused = false;

    void Start()
    {
        UIManager.Instance?.UpdateScoreUI(score);
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
            RestartGame();
        if (isGameOver && Input.GetKeyDown(KeyCode.Q))
            GoToMainMenu();
        if (!isGameOver && Input.GetKeyDown(KeyCode.P))
            TogglePause();
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        UIManager.Instance?.ShowPauseText(isPaused);
    }

    public void AddScore(int amount)
    {
        score += amount;
        persistedScore = score;
        UIManager.Instance?.UpdateScoreUI(score);
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        UIManager.Instance?.ShowGameOver(score);
    }

    public void LevelComplete()
    {
        currentLevelIndex++;
        persistedLevelIndex = currentLevelIndex;

        if (currentLevelIndex < levelScenes.Length)
        {
            StartCoroutine(LoadNextLevelDelayed(0f));
        }
        else
        {
            persistedScore = 0;
            persistedLevelIndex = 0;
            UIManager.Instance?.ShowVictory(score);
            StartCoroutine(LoadMainMenuDelayed(3f));
        }
    }

    IEnumerator LoadNextLevelDelayed(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        string announcement = $"<size=44><color=#FFD700>SECTOR CLEARED</color></size>\n<size=22><color=#555555>- - - - - - - - - - - - - -</color></size>\n<size=28><color=#00CFFF>ENTERING SECTOR {currentLevelIndex + 1}</color></size>";
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(levelScenes[currentLevelIndex], announcement);
        else
            SceneManager.LoadScene(levelScenes[currentLevelIndex]);
    }

    IEnumerator LoadMainMenuDelayed(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene("MainMenu");
        else
            SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        isGameOver = false;
        persistedScore = 0;
        persistedLevelIndex = 0;
        PlayerHealth.ResetHealth();
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelScenes[0]);
    }

    public void GoToMainMenu()
    {
        isGameOver = false;
        persistedScore = 0;
        persistedLevelIndex = 0;
        PlayerHealth.ResetHealth();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() => Application.Quit();

    public int Score => score;
    public int CurrentLevel => currentLevelIndex + 1;
}
