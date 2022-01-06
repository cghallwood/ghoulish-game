using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [HideInInspector] public bool isCompleted = false;
    public int enemiesLeft;
    public float spawnRate;
    public EnemyType[] enemyTypes;
}
