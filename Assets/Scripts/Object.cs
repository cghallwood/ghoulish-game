using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Object
{
    [Range(0f, 1f)] public float spawnChance;
    public GameObject objectPrefab;
    public float spawnTopBound = -3;
    public float spawnBottomBound = -8;

    public Vector2 RandomSpawnPos()
    {
        float randomY = Random.Range(spawnBottomBound, spawnTopBound);

        return new Vector2(CameraMovement.Position.x + 3, randomY);
    }
}
