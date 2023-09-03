using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerTimedWordSpy : LevelManagerWordSpyBase
{
    private void Awake()
    {
        NewBoard(RandomLevelInfo());
        SetScore();
    }

    private void Update()
    {

        if (passedTime >= totalTime)
        {
            passedTime = totalTime;
        }
        else
        {
            passedTime += Time.deltaTime;
            timerVisual.SetFillAmount((totalTime - passedTime) / totalTime);
        }
    }

    override protected void OnCorrectWordSelectedImpl(string word)
    {
        UpdateScore(word);

        if (GetRemainingWords().Count == 0)
        {
            // todo create the new board
            NewBoard(RandomLevelInfo());
            var lvlInfo = GetLevelInfo();
            totalTime += lvlInfo.edgeLength * lvlInfo.wordCount / 2;
        }
    }

    void UpdateScore(string word)
    {
        var lvlInfo = GetLevelInfo();
        score += word.Length * lvlInfo.edgeLength;
        SetScore();
    }

    void SetScore()
    {
        scoreVisual.SetScore(score);
    }

    private LevelInfo RandomLevelInfo()
    {
        int newEdgeLength = rng.Next(5, 8);
        int newWordCount = rng.Next(1, 4);
        return new LevelInfo { edgeLength = newEdgeLength, wordCount = newWordCount };
    }

    // -- 

    [SerializeField] private TimerVisual timerVisual;
    [SerializeField] private ScoreVisual scoreVisual;
    private float passedTime = 0f;
    private float totalTime = 10f;
    private long score = 0;

    private System.Random rng = new System.Random();

}

