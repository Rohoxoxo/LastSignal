using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Waves for this level")]
    public WaveData[] waves;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UIManager.Instance?.UpdateWaveUI(1, waves.Length);
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            yield return new WaitForSeconds(1f);
            UIManager.Instance?.ShowWaveAnnouncement(currentWaveIndex + 1, waves.Length);
            UIManager.Instance?.UpdateWaveUI(currentWaveIndex + 1, waves.Length);
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));

            // Wait until all enemies are dead
            while (enemiesAlive > 0) yield return null;

            currentWaveIndex++;
        }

        yield return new WaitForSeconds(1.5f);
        GameManager.Instance?.LevelComplete();
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            EnemySpawner.Instance?.SpawnEnemy(wave.enemyTypeIndex);
            enemiesAlive++;
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }
}

[System.Serializable]
public class WaveData
{
    public int enemyCount = 5;
    public float spawnInterval = 1f;
    public int enemyTypeIndex = 0;
}
