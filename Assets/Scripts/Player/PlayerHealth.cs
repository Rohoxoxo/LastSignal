using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth { get; private set; }

    private bool isShielded;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        UIManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isShielded) return;
        currentHealth -= amount;
        UIManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);
        if (currentHealth <= 0)
            GameManager.Instance?.GameOver();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UIManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);
    }

    public void ActivateShield(float duration)
    {
        isShielded = true;
        CancelInvoke(nameof(DeactivateShield));
        Invoke(nameof(DeactivateShield), duration);
    }

    void DeactivateShield() => isShielded = false;

    public bool IsShielded => isShielded;
}
