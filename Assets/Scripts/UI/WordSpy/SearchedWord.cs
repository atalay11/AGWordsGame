using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SearchedWord : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Transform strikethrough;

    private void Awake()
    {
        Strikethrough(false);
    }

    public void SetWord(string word)
    {
        textMeshProUGUI.text = word;
    }

    public void Strikethrough(bool active)
    {
        strikethrough.gameObject.SetActive(active);
    }



}
