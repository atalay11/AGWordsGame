using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CrosswordAlgorithm
{
    private static readonly float _failedAttemptLimit = 10;

    public static List<WordVector> GetVectorCrosswordLayout(WordHintDictionary wordHintDictionary)
    {
        Dictionary<WordHintPair, int> unusedWords = wordHintDictionary.WordHintPairs.ToDictionary(x => x, x => 0);

        List<WordVector> wordVectors = new List<WordVector>();

        WordHintPair randomWord = unusedWords.Keys.ToArray()[Random.Range(0, unusedWords.Keys.Count)];
        unusedWords.Remove(randomWord);
        wordVectors.Add(new WordVector(randomWord, Vector2Int.zero, Random.Range(0f, 1) <= 0.5f));

        int failCount = 0;

        List<WordHintPair> lowestFailWords = new List<WordHintPair>();

        WordVector newWordVector;

        while (unusedWords.Count > 0 && failCount < _failedAttemptLimit)
        {
            lowestFailWords.Clear();
            foreach (KeyValuePair<WordHintPair, int> wordPair in unusedWords)
                if (wordPair.Value <= failCount)
                    lowestFailWords.Add(wordPair.Key);

            if (lowestFailWords.Count == 0)
            {
                failCount++;
                continue;
            }

            randomWord = lowestFailWords[Random.Range(0, lowestFailWords.Count)];

            newWordVector = GetValidWordVector(randomWord, wordVectors);
            if (newWordVector.WordHintPair)
            {
                unusedWords.Remove(randomWord);
                wordVectors.Add(newWordVector);
            }
            else
            {
                unusedWords[randomWord]++;
            }
        }

        Debug.Log("Words:");
        for (int i = 0; i < wordVectors.Count; i++)
        {
            WordVector item = wordVectors[i];
            Debug.Log(string.Concat(i, ". ", item.WordHintPair.Word));
        }

        Debug.Log(string.Concat("#Excluded" + unusedWords.Count, " words after ", failCount, " attempt."));
        foreach (KeyValuePair<WordHintPair, int> v in unusedWords)
            Debug.Log(string.Concat(v.Key.Word));

        return wordVectors;
    }

    public static WordVector GetValidWordVector(WordHintPair givenPair, List<WordVector> currentVectorGrid)
    {
        foreach (WordVector wordVector in currentVectorGrid)
        {
            for (int i = 0; i < wordVector.WordHintPair.Word.Length; i++)
            {
                char checkedLetter = wordVector.WordHintPair.Word[i];

                for (int j = 0; j < givenPair.Word.Length; j++)
                {
                    if (char.ToUpper(checkedLetter) == char.ToUpper(givenPair.Word[j]))
                    {
                        bool isDown = !wordVector.IsDown;
                        Vector2Int simOrigin = wordVector.OccupiedPositions[i] + new Vector2Int(isDown ? 0 : -j, isDown ? j : 0);
                        WordVector potWordVector = new WordVector(givenPair, simOrigin, isDown);
                        bool hasOverlap = false;

                        foreach (WordVector dWordVector in currentVectorGrid)
                        {
                            if (dWordVector.CheckForOverlap(potWordVector) && !wordVector.Equals(dWordVector))
                            {
                                hasOverlap = true;
                                break;
                            }
                        }

                        if (!hasOverlap)
                            return potWordVector;
                    }
                }
            }
        }

        Debug.LogError("Failed to create Word Vector.");
        return new WordVector();
    }

    public static char[,] GetEmptyCharacterLayout(Vector2Int dimensions)
    {
        char[,] tempGrid = new char[dimensions.x, dimensions.y];

        for (int x = 0; x < dimensions.x; x++)
            for (int y = 0; y < dimensions.y; y++)
                tempGrid[x, y] = '\0';

        return tempGrid;
    }
}
