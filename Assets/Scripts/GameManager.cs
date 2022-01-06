using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MetricManager metrics;
    public RestartScreen restartScreen;
    public static bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void GameOver(bool isDead)
    {
        metrics.TrackAverageScore(Progression.Score);

        isGameOver = true;
        restartScreen.Setup(isDead);
    }
}
