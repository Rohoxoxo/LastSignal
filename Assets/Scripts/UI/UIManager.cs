using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD")]
    public Slider healthBar;
    public Image healthBarFill;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI scoreText;

    [Header("Screen Flash")]
    public Image screenFlashImage;

    [Header("Game Over Screen")]
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverScoreText;

    [Header("Victory Screen")]
    public GameObject victoryScreen;
    public TextMeshProUGUI victoryScoreText;

    [Header("Sector Cleared Screen")]
    public GameObject sectorClearedScreen;
    public TextMeshProUGUI sectorClearedText;

    [Header("Wave Announcement")]
    public TextMeshProUGUI waveAnnouncementText;

    [Header("Power-up Pickup")]
    public TextMeshProUGUI powerUpPickupText;

    [Header("Pause")]
    public TextMeshProUGUI pauseHintText;
    public GameObject pauseOverlay;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (victoryScreen != null) victoryScreen.SetActive(false);
        if (sectorClearedScreen != null) sectorClearedScreen.SetActive(false);
        if (waveAnnouncementText != null) waveAnnouncementText.gameObject.SetActive(false);
        if (powerUpPickupText != null) powerUpPickupText.gameObject.SetActive(false);
        if (screenFlashImage != null) screenFlashImage.color = new Color(0, 0, 0, 0);
        if (pauseOverlay != null) pauseOverlay.SetActive(false);
    }

    public void UpdateHealthUI(int current, int max)
    {
        if (healthBar == null) return;
        float pct = (float)current / max;
        healthBar.value = pct;

        if (healthBarFill == null) return;
        if (pct > 0.6f)
            healthBarFill.color = new Color(0.1f, 0.9f, 0.2f);       // green
        else if (pct > 0.35f)
            healthBarFill.color = new Color(1f, 0.65f, 0f);           // orange
        else
            healthBarFill.color = new Color(0.95f, 0.1f, 0.1f);       // red
    }

    public void FlashDamage()
    {
        if (screenFlashImage == null) return;
        StopCoroutine(nameof(DoFlash));
        StartCoroutine(DoFlash(new Color(1f, 0f, 0f, 0.35f)));

        if (healthBar != null)
            StartCoroutine(ShakeHealthBar());
    }

    public void FlashHeal()
    {
        if (screenFlashImage == null) return;
        StopCoroutine(nameof(DoFlash));
        StartCoroutine(DoFlash(new Color(0f, 1f, 0.2f, 0.28f)));
    }

    IEnumerator DoFlash(Color flashColor)
    {
        screenFlashImage.color = flashColor;
        float elapsed = 0f;
        while (elapsed < 0.4f)
        {
            elapsed += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(flashColor.a, 0f, elapsed / 0.4f);
            screenFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, a);
            yield return null;
        }
        screenFlashImage.color = new Color(0, 0, 0, 0);
    }

    IEnumerator ShakeHealthBar()
    {
        var rt = healthBar.GetComponent<RectTransform>();
        Vector2 original = rt.anchoredPosition;
        float elapsed = 0f;
        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            float x = original.x + Mathf.Sin(elapsed * 80f) * 5f * (1f - elapsed / 0.3f);
            rt.anchoredPosition = new Vector2(x, original.y);
            yield return null;
        }
        rt.anchoredPosition = original;
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

        if (gameOverScoreText)
        {
            gameOverScoreText.text = $"FINAL SCORE\n<size=150%><color=#FFD700>{score}</color></size>";
        }
        else if (gameOverScreen != null)
        {
            // Create score text inside GameOverScreen if not wired
            var go = new GameObject("GameOverScore");
            go.transform.SetParent(gameOverScreen.transform, false);
            var tmp = go.AddComponent<TMPro.TextMeshProUGUI>();
            tmp.text = $"FINAL SCORE\n<size=150%><color=#FFD700>{score}</color></size>";
            tmp.fontSize = 28;
            tmp.alignment = TMPro.TextAlignmentOptions.Center;
            tmp.color = Color.white;
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(0, -120f);
            rt.sizeDelta = new Vector2(400f, 80f);
            gameOverScoreText = tmp;
        }

        if (scoreText) scoreText.text = $"Score: {score}";
    }

    public void ShowVictory(int score)
    {
        if (victoryScreen) victoryScreen.SetActive(true);
        if (victoryScoreText) victoryScoreText.text = $"Final Score: {score}";
    }

    public void ShowSectorCleared(int nextLevelIndex)
    {
        if (sectorClearedScreen)
        {
            sectorClearedScreen.SetActive(true);
            // Force dark background regardless of Inspector setting
            var bg = sectorClearedScreen.GetComponent<Image>();
            if (bg != null) bg.color = new Color(0f, 0f, 0f, 0.92f);
        }
        if (sectorClearedText)
        {
            sectorClearedText.text = $"SECTOR CLEARED\n— ENTERING SECTOR {nextLevelIndex + 1} —";
            sectorClearedText.color = new Color(1f, 0.85f, 0f); // yellow
            sectorClearedText.fontSize = 72;
        }
    }

    void HideSectorCleared()
    {
        if (sectorClearedScreen) sectorClearedScreen.SetActive(false);
    }

    public void ShowPauseText(bool paused)
    {
        if (pauseHintText != null)
            pauseHintText.text = paused ? "PAUSED — Press P to Resume" : "Press P to Pause";
        if (pauseOverlay != null)
            pauseOverlay.SetActive(paused);
    }

    public void ShowWaveAnnouncement(int wave, int total)
    {
        if (waveAnnouncementText == null) return;
        StopCoroutine(nameof(FadeWaveAnnouncement));
        waveAnnouncementText.text = $"WAVE {wave} / {total}";
        waveAnnouncementText.gameObject.SetActive(true);
        StartCoroutine(FadeWaveAnnouncement());
    }

    IEnumerator FadeWaveAnnouncement()
    {
        yield return new WaitForSeconds(2f);
        if (waveAnnouncementText != null)
            waveAnnouncementText.gameObject.SetActive(false);
    }

    public void ShowPowerUpText(string message, Color color)
    {
        if (powerUpPickupText == null) return;
        StopCoroutine(nameof(FadePowerUpText));
        string hex = ColorUtility.ToHtmlStringRGB(color);
        powerUpPickupText.text = $"<color=#{hex}>{message}</color>";
        powerUpPickupText.color = Color.white;
        powerUpPickupText.gameObject.SetActive(true);
        StartCoroutine(FadePowerUpText());
    }

    IEnumerator FadePowerUpText()
    {
        yield return new WaitForSeconds(2f);
        if (powerUpPickupText != null)
            powerUpPickupText.gameObject.SetActive(false);
    }
}
