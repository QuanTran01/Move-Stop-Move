using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform playerTransform;
    public Vector3 spawnAreaSize;
    public int maxEnemies = 6;
    private int currentEnemyCount = 0;

    void Start()
    {
        SpawnInitialEnemies();
    }

    void Update()
    {
        if (currentEnemyCount < maxEnemies)
        {
            SpawnEnemiesToMaintainCount();
        }
    }

    void SpawnInitialEnemies()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemiesToMaintainCount()
    {
        int enemiesToSpawn = maxEnemies - currentEnemyCount;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomPosition = playerTransform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            0,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        currentEnemyCount++;
    }

    public void OnEnemyDeath()
    {
        currentEnemyCount--;
    }
}
