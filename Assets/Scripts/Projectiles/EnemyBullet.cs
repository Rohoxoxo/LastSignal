using UnityEngine;

// Enemy bullet — attach to the enemy bullet prefab.
// Prefab needs: Rigidbody (Is Kinematic = true), Collider (Is Trigger = true).
public class EnemyBullet : MonoBehaviour
{
    public float speed = 2.7f;
    public int damage = 1;
    public float lifetime = 4f;

    void Start()
    {
        Destroy(gameObject, lifetime);

        // Red emissive glow
        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = new Color(1f, 0.1f, 0.05f);
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", new Color(2f, 0.1f, 0f));
        }

        // Red point light
        var lightGO = new GameObject("EnemyBulletLight");
        lightGO.transform.SetParent(transform);
        lightGO.transform.localPosition = Vector3.zero;
        var l = lightGO.AddComponent<Light>();
        l.type = LightType.Point;
        l.color = new Color(1f, 0.1f, 0.05f);
        l.intensity = 1.8f;
        l.range = 3f;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
