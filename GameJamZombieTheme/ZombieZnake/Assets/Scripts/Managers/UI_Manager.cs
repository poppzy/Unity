using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    [Header("UI")]

    //private
    private TextMeshProUGUI scoreText;
    private GameObject gameOverScreen;
    private int startingPoints = 0;
    private int currentPoints;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        gameOverScreen = GameObject.Find("GameOverScreen");
        gameOverScreen.SetActive(false);
        StartScore();
    }

    void StartScore()
    {
        scoreText.text = startingPoints.ToString();
    }

    public void AddScore(int points)
    {
        currentPoints += points;
        UpdateCurrentPoints();
    }

    void UpdateCurrentPoints()
    {
        scoreText.text = currentPoints.ToString();
    }

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
