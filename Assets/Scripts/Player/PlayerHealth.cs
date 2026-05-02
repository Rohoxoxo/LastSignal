using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth { get; private set; }

    private static int persistedHealth = -1;

    private bool isShielded;
    private ShieldVisual shieldVisual;

    void Awake()
    {
        shieldVisual = GetComponent<ShieldVisual>();
        currentHealth = persistedHealth > 0 ? persistedHealth : maxHealth;
    }

    void Start()
    {
        UIManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isShielded) return;
        currentHealth -= amount;
        persistedHealth = currentHealth;
        UIManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);
        UIManager.Instance?.FlashDamage();
        if (currentHealth <= 0)
            GameManager.Instance?.GameOver();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        persistedHealth = currentHealth;
        UIManager.Instance?.UpdateHealthUI(currentHealth, maxHealth);
        UIManager.Instance?.FlashHeal();
    }

    public void ActivateShield(float duration)
    {
        isShielded = true;
        shieldVisual?.Activate();
        CancelInvoke(nameof(DeactivateShield));
        Invoke(nameof(DeactivateShield), duration);
    }

    void DeactivateShield()
    {
        isShielded = false;
        shieldVisual?.Deactivate();
    }

    public bool IsShielded => isShielded;

    public static void ResetHealth() => persistedHealth = -1;
}
