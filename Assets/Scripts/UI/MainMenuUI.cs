using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("First level scene name")]
    public string firstLevelScene = "Level1";

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(firstLevelScene);
    }

    public void QuitGame() => Application.Quit();
}
