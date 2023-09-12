using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    readonly private float lerpDuration = 2.0f;
    private float lerpT = 0.0f;
    private long m_score = 0;
    private long m_lerpScore = 0;

    const string ScoreText = "Score ";

    private void Awake()
    {
        SetScoreText(0);
    }

    public void SetScore(long score)
    {
        lerpT = 0.0f;
        m_lerpScore = m_score;
        m_score = score;
    }

    private void Update()
    {
        if (m_lerpScore != m_score)
        {
            lerpT += Time.deltaTime / lerpDuration;
            m_lerpScore = (int)Mathf.Lerp(m_lerpScore, m_score, lerpT);
            SetScoreText(m_lerpScore);
        }

    }

    private void SetScoreText(long score)
    {
        m_text.text = ScoreText + score.ToString();
    }

}
