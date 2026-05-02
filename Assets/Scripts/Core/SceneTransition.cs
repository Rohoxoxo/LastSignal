using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    private Image fadeImage;
    private TextMeshProUGUI announceText;
    public float fadeDuration = 0.4f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        gameObject.AddComponent<GraphicRaycaster>();

        // Full screen fade image
        GameObject imgObj = new GameObject("FadeImage");
        imgObj.transform.SetParent(transform, false);
        fadeImage = imgObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.raycastTarget = false;
        RectTransform rt = imgObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        // Announcement text shown during transition
        GameObject textObj = new GameObject("AnnounceText");
        textObj.transform.SetParent(transform, false);
        announceText = textObj.AddComponent<TextMeshProUGUI>();
        announceText.alignment = TextAlignmentOptions.Center;
        announceText.fontSize = 36;
        announceText.fontStyle = FontStyles.Bold;
        announceText.color = new Color(1f, 0.85f, 0f, 0f);
        announceText.text = "";
        RectTransform trt = textObj.GetComponent<RectTransform>();
        trt.anchorMin = new Vector2(0.1f, 0.38f);
        trt.anchorMax = new Vector2(0.9f, 0.62f);
        trt.offsetMin = Vector2.zero;
        trt.offsetMax = Vector2.zero;
    }

    public void LoadScene(string sceneName, string announcement = "")
    {
        StartCoroutine(FadeAndLoad(sceneName, announcement));
    }

    IEnumerator FadeAndLoad(string sceneName, string announcement)
    {
        // Fade to black
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, a);
            if (announceText != null)
                announceText.color = new Color(1f, 0.85f, 0f, a);
            yield return null;
        }

        // Show announcement text while black
        if (announceText != null) announceText.text = announcement;
        yield return new WaitForSecondsRealtime(1.5f);

        SceneManager.LoadScene(sceneName);
        yield return new WaitForEndOfFrame();

        // Fade from black
        t = fadeDuration;
        while (t > 0)
        {
            t -= Time.unscaledDeltaTime;
            float a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, a);
            if (announceText != null)
                announceText.color = new Color(1f, 0.85f, 0f, a);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
        if (announceText != null) { announceText.color = new Color(1f, 0.85f, 0f, 0f); announceText.text = ""; }
    }
}
