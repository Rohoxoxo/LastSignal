using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Enemy Prefabs (index matches WaveData.enemyTypeIndex)")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn ring radius (beyond arena edge)")]
    public float spawnMargin = 13f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnEnemy(int typeIndex = 0)
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
        int idx = Mathf.Clamp(typeIndex, 0, enemyPrefabs.Length - 1);
        Instantiate(enemyPrefabs[idx], GetEdgePosition(), Quaternion.identity);
    }

    // Returns a random position along the four edges of the spawn boundary
    Vector3 GetEdgePosition()
    {
        int side = Random.Range(0, 4); // 0=top, 1=bottom, 2=left, 3=right
        float r = spawnMargin;
        return side switch
        {
            0 => new Vector3(Random.Range(-r, r), 0f, r),
            1 => new Vector3(Random.Range(-r, r), 0f, -r),
            2 => new Vector3(-r, 0f, Random.Range(-r, r)),
            _ => new Vector3(r, 0f, Random.Range(-r, r)),
        };
    }
}
