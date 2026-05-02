using UnityEngine;

public enum PowerUpType { SpeedBoost, DamageUp, Shield, HealthUp }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float duration = 5f;
    public float rotateSpeed = 90f;

    private float startY;
    private Light pointLight;
    private Color glowColor;

    void Start()
    {
        Destroy(gameObject, 8f);
        startY = transform.position.y;

        glowColor = type switch
        {
            PowerUpType.SpeedBoost => new Color(1f, 0.85f, 0f),      // gold — speed/energy
            PowerUpType.DamageUp   => new Color(1f, 0.25f, 0f),      // red-orange — aggressive
            PowerUpType.Shield     => new Color(0.2f, 0.75f, 1f),    // ice blue — protection
            PowerUpType.HealthUp   => new Color(0.1f, 0.95f, 0.25f), // green — life/health
            _                      => Color.white
        };

        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = glowColor;
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", glowColor * 2f);
        }

        var lightGO = new GameObject("PowerUpLight");
        lightGO.transform.SetParent(transform);
        lightGO.transform.localPosition = Vector3.zero;
        pointLight = lightGO.AddComponent<Light>();
        pointLight.type = LightType.Point;
        pointLight.color = glowColor;
        pointLight.intensity = 2.5f;
        pointLight.range = 5f;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.y = startY + Mathf.Sin(Time.time * 2f) * 0.25f;
        transform.position = pos;

        float pulse = 1f + Mathf.Sin(Time.time * 3f) * 0.12f;
        transform.localScale = Vector3.one * pulse;

        if (pointLight != null)
            pointLight.intensity = 2f + Mathf.Sin(Time.time * 4f) * 0.8f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Apply(other.gameObject);
        Destroy(gameObject);
    }

    void Apply(GameObject player)
    {
        switch (type)
        {
            case PowerUpType.SpeedBoost:
                player.GetComponent<PlayerMovement>()?.ApplySpeedBoost(14f, duration);
                UIManager.Instance?.ShowPowerUpText("SPEED BOOST!", glowColor);
                break;
            case PowerUpType.DamageUp:
                player.GetComponent<PlayerShooting>()?.ApplyDamageBoost(5f, duration);
                UIManager.Instance?.ShowPowerUpText("DAMAGE UP!", glowColor);
                break;
            case PowerUpType.Shield:
                player.GetComponent<PlayerHealth>()?.ActivateShield(duration);
                UIManager.Instance?.ShowPowerUpText("SHIELD ACTIVE!", glowColor);
                break;
            case PowerUpType.HealthUp:
                player.GetComponent<PlayerHealth>()?.Heal(2);
                UIManager.Instance?.ShowPowerUpText("+2 HEALTH!", glowColor);
                break;
        }
    }
}
