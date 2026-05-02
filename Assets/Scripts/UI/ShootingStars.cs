using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootingStars : MonoBehaviour
{
    private Canvas canvas;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 1.2f));
            StartCoroutine(ShootStar());
        }
    }

    IEnumerator ShootStar()
    {
        // Create the star streak
        var go = new GameObject("ShootingStar");
        go.transform.SetParent(transform, false);

        var img = go.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0f);

        var rt = go.GetComponent<RectTransform>();
        float length = Random.Range(60f, 180f);
        rt.sizeDelta = new Vector2(length, Random.Range(1.5f, 3f));

        // Spawn from top-right area, move toward bottom-left
        float startX = Random.Range(0f, 960f);
        float startY = Random.Range(100f, 540f);
        rt.anchoredPosition = new Vector2(startX, startY);

        // Angle: diagonal down-left like real shooting stars
        float angle = Random.Range(-35f, -20f);
        rt.localRotation = Quaternion.Euler(0f, 0f, angle);

        float speed = Random.Range(400f, 900f);
        float duration = Random.Range(0.4f, 0.9f);
        float elapsed = 0f;

        // Direction: left and down
        Vector2 dir = new Vector2(-Mathf.Cos(Mathf.Abs(angle) * Mathf.Deg2Rad),
                                   -Mathf.Sin(Mathf.Abs(angle) * Mathf.Deg2Rad));

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Fade in then out
            float alpha = t < 0.2f ? (t / 0.2f) : (1f - ((t - 0.2f) / 0.8f));
            img.color = new Color(0.9f, 0.95f, 1f, Mathf.Clamp01(alpha) * 0.9f);

            rt.anchoredPosition += dir * speed * Time.deltaTime;
            yield return null;
        }

        Destroy(go);
    }
}
