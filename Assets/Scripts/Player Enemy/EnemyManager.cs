using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform playerTransform;
    public Vector3 spawnAreaSize;
    public int maxEnemies = 6;
    public int totalEnemiesToSpawn = 60;
    public TMP_Text enemyCountText;

    private int currentEnemyCount = 0;
    private int spawnedEnemies = 0;
    private List<Transform> enemies = new List<Transform>();  

    void Start()
    {
        Enemy.OnDeath += OnEnemyDeath;
        SpawnInitialEnemies();
        UpdateEnemyCountText();
    }

    void OnDestroy()
    {
        Enemy.OnDeath -= OnEnemyDeath;
    }

    void Update()
    {
        if (currentEnemyCount < maxEnemies && spawnedEnemies < totalEnemiesToSpawn)
        {
            SpawnEnemiesToMaintainCount();
        }
    }

    void SpawnInitialEnemies()
    {
        for (int i = 0; i < Mathf.Min(maxEnemies, totalEnemiesToSpawn); i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemiesToMaintainCount()
    {
        int enemiesToSpawn = Mathf.Min(maxEnemies - currentEnemyCount, totalEnemiesToSpawn - spawnedEnemies);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (spawnedEnemies >= totalEnemiesToSpawn)
        {
            return;
        }

        float spawnDistance = Random.Range(10f, spawnAreaSize.x / 2);
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 randomPosition = playerTransform.position + new Vector3(randomDirection.x * spawnDistance, 0, randomDirection.y * spawnDistance);

        GameObject enemyObject = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        Transform enemyTransform = enemyObject.transform;

        enemies.Add(enemyTransform);
     
        currentEnemyCount++;
        spawnedEnemies++;
        UpdateEnemyCountText();
    }

    void OnEnemyDeath(Transform enemyTransform)
    {
        if (enemies.Contains(enemyTransform))
        {
            enemies.Remove(enemyTransform);
        }

        currentEnemyCount--;
        UpdateEnemyCountText();
    }

    void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Alive: " + (totalEnemiesToSpawn - (spawnedEnemies - currentEnemyCount));
        }
    }
}
