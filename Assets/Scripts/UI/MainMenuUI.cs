using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("First level scene name")]
    public string firstLevelScene = "Level1";

    [Header("Power-up Guide Labels")]
    public TextMeshProUGUI speedLabel;
    public TextMeshProUGUI damageLabel;
    public TextMeshProUGUI shieldLabel;
    public TextMeshProUGUI healthLabel;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(CreateCursorTexture(), new Vector2(16, 16), CursorMode.Auto);

        if (speedLabel  != null) speedLabel.color  = new Color(1f, 0.85f, 0f);
        if (damageLabel != null) damageLabel.color = new Color(1f, 0.25f, 0f);
        if (shieldLabel != null) shieldLabel.color = new Color(0.2f, 0.75f, 1f);
        if (healthLabel != null) healthLabel.color = new Color(0.1f, 0.95f, 0.25f);
    }

    Texture2D CreateCursorTexture()
    {
        int size = 32;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color clear = new Color(0, 0, 0, 0);
        Color cyan = new Color(0f, 0.9f, 1f, 1f);
        Color dimCyan = new Color(0f, 0.6f, 0.8f, 0.6f);

        // Fill transparent
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                tex.SetPixel(x, y, clear);

        int cx = size / 2;
        int cy = size / 2;
        int outerR = 10;
        int innerR = 5;
        int gapR = 4;

        // Draw outer circle
        for (int x = 0; x < size; x++)
        for (int y = 0; y < size; y++)
        {
            float dist = Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
            if (dist >= outerR - 1.2f && dist <= outerR + 0.5f)
                tex.SetPixel(x, y, cyan);
            if (dist >= innerR - 0.8f && dist <= innerR + 0.5f)
                tex.SetPixel(x, y, dimCyan);
        }

        // Draw crosshair lines with gap in center
        for (int i = 0; i < size; i++)
        {
            float distFromCenter = Mathf.Abs(i - cx);
            if (distFromCenter < gapR || distFromCenter > outerR) continue;
            tex.SetPixel(i, cy, cyan);
            tex.SetPixel(cx, i, cyan);
        }

        // Bright center dot
        tex.SetPixel(cx,     cy,     cyan);
        tex.SetPixel(cx + 1, cy,     cyan);
        tex.SetPixel(cx - 1, cy,     cyan);
        tex.SetPixel(cx,     cy + 1, cyan);
        tex.SetPixel(cx,     cy - 1, cyan);

        tex.Apply();
        return tex;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(firstLevelScene, "");
        else
            SceneManager.LoadScene(firstLevelScene);
    }

    public void QuitGame() => Application.Quit();
}
