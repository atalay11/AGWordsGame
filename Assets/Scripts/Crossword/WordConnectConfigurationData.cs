using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WordConnectConfigurationData : ScriptableObject
{
    [SerializeField]
    private WordHintDictionary _dictionaryAsset;

    public WordHintDictionary DictionaryAsset => _dictionaryAsset;

    [SerializeField]
    private int _scorePerWord = 100;

    public int ScorePerWord => _scorePerWord;


    [SerializeField]
    private List<WordVector> _wordVectors;

    public List<WordVector> WordVectors => _wordVectors;

    private char[,] _crosswordLetters;

    public char[,] CrosswordLetters => _crosswordLetters;

    [SerializeField]
    private int _gridWidth;

    [SerializeField]
    private string[] _flattenedLetterLayout;

    public WordConnectLayout ReturnLayout() => new WordConnectLayout(null, _crosswordLetters, _wordVectors.Select(x => x.DeepCopy()).ToList());

    public void SetWordVectors(List<WordVector> wordVectors) => _wordVectors = wordVectors;

    public void SetCrosswordLetters(char[,] crosswordLetters)
    {
        _crosswordLetters = crosswordLetters;
        _gridWidth = _crosswordLetters.GetLength(0);

        _flattenedLetterLayout = new string[_crosswordLetters.GetLength(0) * _crosswordLetters.GetLength(1)];
        int iteration = 0;
        for (int x = crosswordLetters.GetLength(1) - 1; x >= 0; x--)
        {
            for (int y = 0; y < crosswordLetters.GetLength(0); y++)
            {
                _flattenedLetterLayout[iteration] = crosswordLetters[y, x].ToString();
                iteration++;
            }
        }
    }

    public float CalculatePoints(float seconds)
    {
        // Something else
        float finalScore = 500 / seconds;
        return finalScore;
    }

    public bool LetterHasOverlap(string word, int letterIndex)
    {
        WordVector wordVectorToCheck = _wordVectors.Find(wordVector => wordVector.WordHintPair.Word.ToLower() == word.ToLower());
        Vector2Int letterPosition = wordVectorToCheck.OccupiedPositions[letterIndex];

        foreach (WordVector wordVector in WordVectors)
        {
            if (wordVector.WordHintPair.Word == wordVectorToCheck.WordHintPair.Word) continue;
            if (wordVector.OccupiedPositions.Contains(letterPosition)) return true;

        }
        return false;
    }

    // Return the overlapping word and its index.
    public (string, int) GetLetterOverlap(string word, int letterIndex)
    {
        WordVector wordVectorToCheck = _wordVectors.Find(wordVector => wordVector.WordHintPair.Word.ToLower() == word.ToLower());
        _wordVectors.ForEach(wordVector => wordVector.CalculatePositions());

        Vector2Int letterPosition = wordVectorToCheck.OccupiedPositions[letterIndex];
        foreach (WordVector wordVector in WordVectors)
        {
            if (wordVector.WordHintPair.Word == wordVectorToCheck.WordHintPair.Word)
                continue;

            if (wordVector.OccupiedPositions.Contains(letterPosition))
                return (wordVector.WordHintPair.Word, wordVector.OccupiedPositions.IndexOf(letterPosition));

        }
        return (null, 0);
    }

}
