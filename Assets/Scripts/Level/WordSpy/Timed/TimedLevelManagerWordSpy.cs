using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerTimedWordSpy : LevelManagerWordSpyBase
{
    protected override void StartImpl()
    {
        NewBoard(RandomLevelInfo());
        m_curGameData = PersistanceManager.Instance.GetArcadeWordSpyGameData();
        Debug.Log($"Highest Score: {m_curGameData.highestScore}.");
    }

    private void Update()
    {

        if (m_passedTime >= m_totalTime)
        {
            if (m_gameOver == false)
            {
                m_passedTime = m_totalTime;
                GameOver();
            }


            m_gameOver = true;
        }
        else
        {
            m_passedTime += Time.deltaTime;
            m_timerVisual.SetFillAmount((m_totalTime - m_passedTime) / m_totalTime);
        }
    }

    override protected void OnCorrectWordSelectedImpl(string word)
    {
        if (m_gameOver)
            return;

        UpdateScore(word);

        if (GetRemainingWords().Count == 0)
        {
            NewBoard(RandomLevelInfo());
            var lvlInfo = GetLevelInfo();
            m_totalTime += lvlInfo.edgeLength * lvlInfo.wordCount / 2;
        }
    }

    void UpdateScore(string word)
    {
        var lvlInfo = GetLevelInfo();
        m_score += word.Length * lvlInfo.edgeLength * 100;
        SetScore();
    }

    void SetScore()
    {
        m_scoreVisual.SetScore(m_score);
    }

    private LevelInfo RandomLevelInfo()
    {
        int newEdgeLength = m_rng.Next(5, 8);
        int newWordCount = m_rng.Next(1, 4);
        return new LevelInfo { edgeLength = newEdgeLength, wordCount = newWordCount };
    }

    private void SaveGameData()
    {
        bool change = false;
        if (m_score > m_curGameData.highestScore)
        {
            m_curGameData.highestScore = m_score;
            Debug.Log($"New Highest Score: {m_curGameData.highestScore}.");
            change = true;
        }



        if (change)
        {
            PersistanceManager.Instance.SetArcadeWordSpyGameData(m_curGameData);
        }
    }

    private void GameOver()
    {
        Debug.Log("GameOver!");
        SaveGameData();

        var args = new GameOverEventArgs { score = m_score, highScore = m_curGameData.highestScore };
        OnGameOver?.Invoke(this, args);
    }


    public void PlayAgain()
    {
        m_passedTime = 0f;
        m_totalTime = 10f;
        m_score = 0;
        m_gameOver = false;
        SetScore();
        NewBoard(RandomLevelInfo());
    }

    // Events

    public EventHandler<GameOverEventArgs> OnGameOver;
    public class GameOverEventArgs : EventArgs
    {
        public long score;
        public long highScore;
    }


    // Variables

    [SerializeField] private TimerVisual m_timerVisual;
    [SerializeField] private ScoreVisual m_scoreVisual;
    private float m_passedTime = 0f;
    private float m_totalTime = 10f;
    private long m_score = 0;
    private bool m_gameOver = false;
    ArcadeWordSpyGameData m_curGameData = new ArcadeWordSpyGameData();

    private System.Random m_rng = new System.Random();

}

