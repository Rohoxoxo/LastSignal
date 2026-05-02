using UnityEngine;
using UnityEngine.UI;

public class AnimatedStarfield : MonoBehaviour
{
    public int starCount = 250;
    public float maxSpeed = 800f;

    private struct Star
    {
        public RectTransform rt;
        public Image img;
        public Vector2 direction;
        public float distance;
        public float speed;
        public float maxDist;
        public float brightness;
    }

    private Star[] stars;
    private Sprite circleSprite;
    private float screenH = 540f;
    private float screenW = 960f;

    void Start()
    {
        circleSprite = CreateCircleSprite();

        var rt = GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        stars = new Star[starCount];
        for (int i = 0; i < starCount; i++)
            stars[i] = CreateStar(true);
    }

    Star CreateStar(bool randomStart)
    {
        var go = new GameObject("s");
        go.transform.SetParent(transform, false);

        var img = go.AddComponent<Image>();
        img.sprite = circleSprite;
        img.color = new Color(1, 1, 1, 0);

        var srt = go.GetComponent<RectTransform>();
        srt.sizeDelta = Vector2.one * 2f;
        srt.anchorMin = srt.anchorMax = new Vector2(0.5f, 0.5f);
        srt.anchoredPosition = Vector2.zero;

        float angle = Random.Range(0f, Mathf.PI * 2f);
        float maxDist = Mathf.Sqrt(screenW * screenW + screenH * screenH);
        float startDist = randomStart ? Random.Range(0f, maxDist * 0.8f) : 0f;

        return new Star
        {
            rt = srt,
            img = img,
            direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)),
            distance = startDist,
            speed = Random.Range(60f, 160f),
            maxDist = maxDist,
            brightness = Random.Range(0.6f, 1f)
        };
    }

    void Update()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            ref var s = ref stars[i];

            // Accelerate as it moves outward
            s.speed += 180f * Time.deltaTime;
            s.speed = Mathf.Min(s.speed, maxSpeed);
            s.distance += s.speed * Time.deltaTime;

            float t = s.distance / s.maxDist;

            // Grow from tiny to bigger
            float size = Mathf.Lerp(1f, 8f, t);
            s.rt.sizeDelta = Vector2.one * size;

            // Fade in at start, full brightness in middle/end
            float alpha = Mathf.Clamp01(t * 4f) * s.brightness;
            s.img.color = new Color(0.9f, 0.95f, 1f, alpha);

            s.rt.anchoredPosition = s.direction * s.distance;

            // Reset when out of screen — keep object, just reposition
            if (t >= 1f)
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);
                s.direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                s.distance = Random.Range(0f, s.maxDist * 0.15f);
                s.speed = Random.Range(60f, 160f);
                s.brightness = Random.Range(0.6f, 1f);
                s.rt.sizeDelta = Vector2.one;
                s.img.color = new Color(1, 1, 1, 0);
            }
        }
    }

    Sprite CreateCircleSprite()
    {
        int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        float center = size / 2f;
        float radius = size / 2f - 1f;

        for (int x = 0; x < size; x++)
        for (int y = 0; y < size; y++)
        {
            float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
            float a = Mathf.Clamp01(1f - (dist - (radius - 2f)) / 2f);
            tex.SetPixel(x, y, new Color(1, 1, 1, a));
        }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), Vector2.one * 0.5f);
    }
}
