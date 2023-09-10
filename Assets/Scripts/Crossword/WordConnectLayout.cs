using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WordConnectLayout
{
    public GameObject[,] GridTiles { get; private set; }

    public char[,] GridLetters { get; private set; }

    [SerializeField]
    public List<WordVector> WordVectors { get; private set; }

    public void SetGridTiles(GameObject[,] gridTiles) => GridTiles = gridTiles;

    public void SetGridLetters(char[,] gridLetters) => GridLetters = gridLetters;

    public void SetWordVectors(List<WordVector> wordVectors) => WordVectors = wordVectors;

    public WordConnectLayout(GameObject[,] gridTiles, char[,] gridLetters, List<WordVector> wordVectors)
    {
        GridTiles = gridTiles;
        GridLetters = gridLetters;
        WordVectors = wordVectors;
    }

    public void UpdateLayout(WordConnectLayout layout)
    {
        GridTiles = layout.GridTiles;
        GridLetters = layout.GridLetters;
        WordVectors = layout.WordVectors;
    }
}