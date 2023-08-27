using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectedWordText : MonoBehaviour
{
    TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        tmpText.text = "";
    }


    private void Start()
    {
        LetterSelectionChecker.Instance.OnWordSelected += LetterSelectionChecker_OnSelectedWordChanged;
    }

    private void LetterSelectionChecker_OnSelectedWordChanged(object sender, LetterSelectionChecker.OnWordSelectedEventArgs e)
    {
        tmpText.text = e.word;
    }
}
