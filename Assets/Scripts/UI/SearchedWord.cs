using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SearchedWord : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    public void SetWord(string word)
    {
        textMeshProUGUI.text = word;
    }

    public void CrossWord()
    {

    }



}
