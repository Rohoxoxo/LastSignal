using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public static PowerUpSpawner Instance;

    [Header("Prefabs (SpeedBoost, DamageUp, Shield)")]
    public GameObject[] powerUpPrefabs;

    [Range(0f, 1f)]
    public float dropChance = 0.10f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TrySpawnPowerUp(Vector3 position)
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0) return;
        if (Random.value > dropChance) return;

        int idx = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[idx], position + Vector3.up * 0.5f, Quaternion.identity);
    }
}
