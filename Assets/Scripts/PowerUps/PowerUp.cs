using UnityEngine;

public enum PowerUpType { SpeedBoost, DamageUp, Shield }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float duration = 5f;
    public float rotateSpeed = 90f;

    void Start()
    {
        Destroy(gameObject, 8f);
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
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
                break;
            case PowerUpType.DamageUp:
                player.GetComponent<PlayerShooting>()?.ApplyDamageBoost(5f, duration);
                break;
            case PowerUpType.Shield:
                player.GetComponent<PlayerHealth>()?.ActivateShield(duration);
                break;
        }
    }
}
