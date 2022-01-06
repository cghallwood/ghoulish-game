using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RestartScreen : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public GameObject statUI;

    public void Setup(bool isDead)
    {
        string endWord = isDead ? "died" : "escaped";

        gameOverText.text = $"You {endWord} with";
        scoreText.text = Progression.Score.ToString() + " gold";
        statUI.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        GameManager.isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
