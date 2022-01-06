using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public static int EnemyCount;

    public int enemyMax;
    public Wave[] waves;

    [Header("Spawn Position")] 
    public float maxSpawnY;
    public float maxSpawnX;
    public float minSpawnY;
    public float minSpawnX;

    private Wave currentWave;
    private float waveTime;
    private bool isProgressing;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        waveTime = Progression.TimeLeft;
        isProgressing = false;
        EnemyCount = 0;
        currentWave = waves[0];
        StartCoroutine(StartSpawner());
    }

    private void Update()
    {
        // Every 30 seconds, advance wave if player has not done so yet
        float currTime = Progression.TimeLeft;
        if (currTime <= waveTime - 30f)
        {
            if (!isProgressing)
                ChangeWave();

            waveTime = currTime;
            isProgressing = false;
        }
    }

    public void RemoveEnemy()
    {
        EnemyCount--;
        currentWave.enemiesLeft--;

        if (currentWave.enemiesLeft <= 0)
            ChangeWave();
    }

    private void ChangeWave()
    {
        currentWave.isCompleted = true;

        for (int i = 0; i < waves.Length; i++)
        {
            if (!waves[i].isCompleted)
            {
                Debug.Log($"Wave {i} completed");
                currentWave = waves[i];
                return;
            }
        }

        isProgressing = true;
    }

    private IEnumerator StartSpawner()
    {
        while (!GameManager.isGameOver)
        {
            float spawnTime = EnemyCount + currentWave.spawnRate;
            yield return new WaitForSeconds(spawnTime);

            if (GameManager.isGameOver)
                yield break;

            foreach (EnemyType enemyType in currentWave.enemyTypes)
            {
                if (Random.value <= enemyType.spawnChance && EnemyCount < enemyMax)
                {
                    SpawnEnemy(enemyType.enemyPrefab);
                }
            }
        }
    }

    // Spawn enemy at a random position within the camera view
    private void SpawnEnemy(GameObject enemyPrefab)
    {
        float randomY = CameraMovement.Position.y + Random.Range(minSpawnY, maxSpawnY);
        float randomX = CameraMovement.Position.x + Random.Range(minSpawnX, maxSpawnX);
        Vector2 spawnPos = new Vector2(randomX, randomY);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        EnemyCount++;
    }
}
