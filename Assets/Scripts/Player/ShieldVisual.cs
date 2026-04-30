using UnityEngine;

public class ShieldVisual : MonoBehaviour
{
    private GameObject shieldBubble;
    private Light shieldLight;
    private bool active;

    void Awake()
    {
        shieldBubble = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        shieldBubble.transform.SetParent(transform);
        shieldBubble.transform.localPosition = Vector3.zero;
        shieldBubble.transform.localScale = Vector3.one * 2.2f;

        // Remove collider so it doesn't interfere with physics
        Destroy(shieldBubble.GetComponent<Collider>());

        // Build a transparent blue emissive material
        var mat = new Material(Shader.Find("Standard"));
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
        mat.color = new Color(0.1f, 0.5f, 1f, 0.22f);
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", new Color(0f, 0.6f, 1.5f));
        shieldBubble.GetComponent<Renderer>().material = mat;

        // Soft blue light
        var lightGO = new GameObject("ShieldLight");
        lightGO.transform.SetParent(transform);
        lightGO.transform.localPosition = Vector3.zero;
        shieldLight = lightGO.AddComponent<Light>();
        shieldLight.type = LightType.Point;
        shieldLight.color = new Color(0.1f, 0.5f, 1f);
        shieldLight.intensity = 2f;
        shieldLight.range = 6f;

        shieldBubble.SetActive(false);
        shieldLight.enabled = false;
    }

    void Update()
    {
        if (!active) return;
        // Slow rotation and pulse to make it feel alive
        shieldBubble.transform.Rotate(15f * Time.deltaTime, 25f * Time.deltaTime, 0f);
        float pulse = 1f + Mathf.Sin(Time.time * 4f) * 0.06f;
        shieldBubble.transform.localScale = Vector3.one * 2.2f * pulse;
        if (shieldLight != null)
            shieldLight.intensity = 2f + Mathf.Sin(Time.time * 5f) * 0.5f;
    }

    public void Activate()
    {
        active = true;
        shieldBubble.SetActive(true);
        if (shieldLight != null) shieldLight.enabled = true;
    }

    public void Deactivate()
    {
        active = false;
        shieldBubble.SetActive(false);
        if (shieldLight != null) shieldLight.enabled = false;
    }
}
