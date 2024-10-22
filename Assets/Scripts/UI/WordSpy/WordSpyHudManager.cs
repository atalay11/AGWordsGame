using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class WordSpyHudManager : MonoBehaviour
{
    [SerializeField] private Transform searchedWordPrefab;
    [SerializeField] private Transform hud;
    [SerializeField] private LevelManagerWordSpy levelManager; // this is not singleton

    private Dictionary<string, Transform> searchedWordDict;

    private void Awake()
    {
        searchedWordDict = new Dictionary<string, Transform>();

        levelManager.OnSelectedWords += OnSelectedWords;
        LetterSelectionChecker.Instance.OnWordSelected += OnWordSelected;
    }

    private void OnSelectedWords(object sender, LevelManagerWordSpy.OnSelectedWordsEventArgs e)
    {
        foreach (var (_, item) in searchedWordDict)
        {
            Destroy(item.gameObject);
        }

        searchedWordDict.Clear();

        foreach (var word in e.selectedWords)
        {
            var searchedWordTransform = Instantiate(searchedWordPrefab, Vector3.zero, Quaternion.identity, hud);
            searchedWordTransform.GetComponent<SearchedWord>().SetWord(word);
            searchedWordDict[word] = searchedWordTransform;
        }
    }

    private void OnWordSelected(object sender, LetterSelectionChecker.OnWordSelectedEventArgs e)
    {
        if (searchedWordDict.ContainsKey(e.word))
        {
            var searchedWord = searchedWordDict[e.word];
            searchedWord.GetComponent<SearchedWord>().Strikethrough(true);
        }
    }

}
