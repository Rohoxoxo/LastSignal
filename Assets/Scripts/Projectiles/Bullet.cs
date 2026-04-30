using UnityEngine;

// Player bullet — attach to the player bullet prefab.
// Prefab needs: Rigidbody (Is Kinematic = true), Collider (Is Trigger = true).
public class Bullet : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;
    [HideInInspector] public float damageMultiplier = 1f;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);

        Color coreColor = new Color(0.4f, 1f, 1f);
        Color glowColor = new Color(0f, 1.8f, 2.2f);

        // Core bullet — bright cyan
        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = coreColor;
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", glowColor);
        }

        // Halo — larger semi-transparent sphere to fake bloom
        var halo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        halo.transform.SetParent(transform);
        halo.transform.localPosition = Vector3.zero;
        halo.transform.localScale = Vector3.one * 2.6f;
        Destroy(halo.GetComponent<Collider>());

        var haloMat = new Material(Shader.Find("Standard"));
        haloMat.SetFloat("_Mode", 3);
        haloMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        haloMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        haloMat.SetInt("_ZWrite", 0);
        haloMat.DisableKeyword("_ALPHATEST_ON");
        haloMat.EnableKeyword("_ALPHABLEND_ON");
        haloMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        haloMat.renderQueue = 3000;
        haloMat.color = new Color(0f, 0.85f, 1f, 0.18f);
        haloMat.EnableKeyword("_EMISSION");
        haloMat.SetColor("_EmissionColor", new Color(0f, 1f, 1.5f));
        halo.GetComponent<Renderer>().material = haloMat;

        // Point light for environment lighting
        var lightGO = new GameObject("BulletLight");
        lightGO.transform.SetParent(transform);
        lightGO.transform.localPosition = Vector3.zero;
        var l = lightGO.AddComponent<Light>();
        l.type = LightType.Point;
        l.color = new Color(0f, 0.9f, 1f);
        l.intensity = 2.2f;
        l.range = 4f;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.TakeDamage(Mathf.RoundToInt(damage * damageMultiplier));
            Destroy(gameObject);
        }
    }
}
