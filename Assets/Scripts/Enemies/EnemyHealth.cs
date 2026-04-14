using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead;

    void Awake() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        PowerUpSpawner.Instance?.TrySpawnPowerUp(transform.position);
        WaveManager.Instance?.OnEnemyKilled();
        GameManager.Instance?.AddScore(10);
        Destroy(gameObject);
    }
}
