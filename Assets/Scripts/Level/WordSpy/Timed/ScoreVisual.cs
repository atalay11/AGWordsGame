using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;

    const string ScoreText = "Score ";

    public void SetScore(long score)
    {
        m_text.text = ScoreText + score.ToString();
    }
}
