using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPopUp : MonoBehaviour
{
    private void Awake()
    {
        gameOverVisual.gameObject.SetActive(false);
    }

    private void Start()
    {
        levelManager.OnGameOver += LevelManager_OnGameOver;
    }

    private void LevelManager_OnGameOver(object sender, LevelManagerTimedWordSpy.GameOverEventArgs e)
    {
        gameOverVisual.gameObject.SetActive(true);
        SetHighScore(e.highScore);
        SetScore(e.score);
    }

    public void OnPlayAgainButtonPressed()
    {
        Debug.Log("Play Again!");
        levelManager.PlayAgain();
        gameOverVisual.gameObject.SetActive(false);
    }

    private void SetHighScore(long score)
    {
        highScoreText.text = highScoreString + score.ToString();
    }

    private void SetScore(long score)
    {
        scoreText.text = scoreString + score.ToString();
    }

    // --

    [SerializeField] private LevelManagerTimedWordSpy levelManager;
    [SerializeField] private Transform gameOverVisual;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI scoreText;

    readonly string highScoreString = "High Score\n";
    readonly string scoreString = "Score\n";

}
