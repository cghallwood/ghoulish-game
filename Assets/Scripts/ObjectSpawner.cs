using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Object[] objects;
    public float spawnInterval;

    private float _totalChance;
    private float _currInterval;

    private void Start()
    {
        _currInterval = spawnInterval;
        foreach (Object item in objects)
        {
            _totalChance += item.spawnChance;
        }
    }

    private void Update()
    {
        if (PlayerController.Position.x >= _currInterval)
        {
            SpawnRandomObject();
            _currInterval += spawnInterval;
        }
    }

    private void SpawnRandomObject()
    {
        float currentChance = 0f;
        float randomChance = Random.Range(0f, _totalChance);

        foreach (Object item in objects)
        {
            currentChance += item.spawnChance;
            if (randomChance <= currentChance)
            {
                Instantiate(item.objectPrefab, item.RandomSpawnPos(), Quaternion.identity);
                return;
            }
        }
    }
}
