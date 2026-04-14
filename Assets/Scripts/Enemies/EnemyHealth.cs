using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    void Awake() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        PowerUpSpawner.Instance?.TrySpawnPowerUp(transform.position);
        WaveManager.Instance?.OnEnemyKilled();
        GameManager.Instance?.AddScore(10);
        Destroy(gameObject);
    }
}
