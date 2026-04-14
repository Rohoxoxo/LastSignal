using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD")]
    public Slider healthBar;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI scoreText;

    [Header("Game Over Screen")]
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverScoreText;

    [Header("Victory Screen")]
    public GameObject victoryScreen;
    public TextMeshProUGUI victoryScoreText;

    [Header("Sector Cleared Screen")]
    public GameObject sectorClearedScreen;
    public TextMeshProUGUI sectorClearedText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (victoryScreen != null) victoryScreen.SetActive(false);
        if (sectorClearedScreen != null) sectorClearedScreen.SetActive(false);
    }

    public void UpdateHealthUI(int current, int max)
    {
        if (healthBar) healthBar.value = (float)current / max;
    }

    public void UpdateWaveUI(int wave, int total)
    {
        if (waveText) waveText.text = $"Wave {wave} / {total}";
    }

    public void UpdateScoreUI(int score)
    {
        if (scoreText) scoreText.text = $"Score: {score}";
    }

    public void ShowGameOver(int score)
    {
        if (gameOverScreen) gameOverScreen.SetActive(true);
        if (gameOverScoreText) gameOverScoreText.text = $"Score: {score}";
    }

    public void ShowVictory(int score)
    {
        if (victoryScreen) victoryScreen.SetActive(true);
        if (victoryScoreText) victoryScoreText.text = $"Final Score: {score}";
    }

    public void ShowSectorCleared(int nextLevelIndex)
    {
        if (sectorClearedScreen) sectorClearedScreen.SetActive(true);
        if (sectorClearedText)
            sectorClearedText.text = $"SECTOR CLEARED\nEntering Sector {nextLevelIndex + 1}...";
        Invoke(nameof(HideSectorCleared), 2.5f);
    }

    void HideSectorCleared()
    {
        if (sectorClearedScreen) sectorClearedScreen.SetActive(false);
    }
}
