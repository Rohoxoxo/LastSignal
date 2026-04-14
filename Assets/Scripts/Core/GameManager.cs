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

    void Start()
    {
        UIManager.Instance?.UpdateScoreUI(score);
    }

    public void AddScore(int amount)
    {
        score += amount;
        persistedScore = score;
        UIManager.Instance?.UpdateScoreUI(score);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        UIManager.Instance?.ShowGameOver(score);
    }

    public void LevelComplete()
    {
        currentLevelIndex++;
        persistedLevelIndex = currentLevelIndex;

        if (currentLevelIndex < levelScenes.Length)
        {
            UIManager.Instance?.ShowSectorCleared(currentLevelIndex);
            Invoke(nameof(LoadNextLevel), 3f);
        }
        else
        {
            persistedScore = 0;
            persistedLevelIndex = 0;
            Invoke(nameof(LoadMainMenu), 3f);
            UIManager.Instance?.ShowVictory(score);
        }
    }

    void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelScenes[currentLevelIndex]);
    }

    void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        persistedScore = 0;
        persistedLevelIndex = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelScenes[0]);
    }

    public void QuitGame() => Application.Quit();

    public int Score => score;
    public int CurrentLevel => currentLevelIndex + 1;
}
