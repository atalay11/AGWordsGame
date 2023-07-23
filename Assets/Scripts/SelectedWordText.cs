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
        LetterSelectionChecker.Instance.OnSelectedWordChanged += LetterSelectionChecker_OnSelectedWordChanged;
        GameInput.Instance.OnSelectReleaseAction += GameInput_OnSelectReleaseAction;
    }

    private void LetterSelectionChecker_OnSelectedWordChanged(object sender, LetterSelectionChecker.OnSelectedWordChangedEventArgs e)
    {
        tmpText.text = e.word;
    }

    private void GameInput_OnSelectReleaseAction(object sender, EventArgs e)
    {
        tmpText.text = "";
    }

}
