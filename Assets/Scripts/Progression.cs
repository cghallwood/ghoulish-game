using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Progression : MonoBehaviour
{
    public static Progression Instance;
    public static int Score;
    public static float TimeLeft;
    public static int EnemyBuff;
    public static bool TimeOver;

    public MetricManager metrics;
    public TextMeshProUGUI bonusText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject statUI;
    public GameObject finishUI;
    public GameObject finishPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        bonusText.enabled = false;
        Score = 0;
        TimeLeft = 180f;
        TimeOver = false;
    }

    private void Update()
    {
        if (!GameManager.isGameOver && !TimeOver)
            CountDown();
    }

    // Increase score by a given amount
    public void AddScore(int score)
    {
        Score += score;
        scoreText.text = $"{Score}";
    }

    // Set the player's bonus
    public void SetBonus(int amount, string name)
    {
        bonusText.SetText($"+{amount} {name}");
        StartCoroutine(DisplayBonus());
    }

    // Show player's bonus on screen
    public IEnumerator DisplayBonus()
    {
        bonusText.enabled = true;
        yield return new WaitForSeconds(1f);
        bonusText.enabled = false;
    }

    public void Escape()
    {
        metrics.TrackPlayerEscapes();
        GameManager.Instance.GameOver(false);

        Enemy[] enemiesToDestroy = FindObjectsOfType<Enemy>();
        Projectile[] projectilesToDestroy = FindObjectsOfType<Projectile>();

        foreach (Enemy enemy in enemiesToDestroy)
        {
            enemy.Die();
        }

        foreach (Projectile projectile in projectilesToDestroy)
        {
            projectile.Impact();
        }
    }

    // Decrease timer
    private void CountDown()
    {
        TimeLeft -= Time.deltaTime;

        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            TimeOver = true;
            finishUI.SetActive(true);
            Instantiate(finishPrefab, new Vector2(PlayerController.Position.x + 4, -0.7f), Quaternion.identity);
        }

        string minutes = Mathf.Floor(TimeLeft / 60f).ToString();
        string seconds = Mathf.FloorToInt(TimeLeft % 60).ToString("00");

        timerText.text = minutes + ":" + seconds;
    }
}
