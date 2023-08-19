using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class WordSpyHudManager : MonoBehaviour
{
    [SerializeField] private Transform searchedWordPrefab;
    [SerializeField] private Transform hud;

    private List<Transform> searchedWords;

    private void Awake()
    {
        searchedWords = new List<Transform>();
    }

    void Start()
    {
        string[] stringArr = {
            "Interdependence",
            "Instrumentation",
            "Insubordination",
            "Kindheartedness",
            "Kinesthetically",
            "Kaleidoscopical",
            "Lightheadedness",
            "Lackadaisically",
            "Lexicographical",
            "Misappreciation",
            "Marginalization",
            "Materialization"};

        foreach (var item in stringArr)
        {
            var searchedWord = Instantiate(searchedWordPrefab, Vector3.zero, Quaternion.identity, hud);
            searchedWord.GetComponent<SearchedWord>().SetWord(item);
            searchedWords.Add(searchedWord);
        }

    }
}
