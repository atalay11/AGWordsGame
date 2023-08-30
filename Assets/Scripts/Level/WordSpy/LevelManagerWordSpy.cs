using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelManagerWordSpy : LevelManagerBase
{
    [SerializeField] private GameObject board;
    [SerializeField] private int defaultEdgeLength;
    [SerializeField] private int defaultWordCount;

    public EventHandler<OnSelectedWordsEventArgs> OnSelectedWords;
    public class OnSelectedWordsEventArgs : EventArgs
    {
        public List<string> selectedWords;
    }
    
    private void Awake()
    {
        NewBoard(defaultEdgeLength);
    }

    public void NewBoard(int edgeLength)
    {
        board.GetComponent<BoardWordSpy>().ResetWithNewEdgeLength(edgeLength);
    }

    private void Start()
    {
        UpdateBoardWordsRandomly(defaultWordCount);
    }

    private void UpdateBoardWordsRandomly(int wordCount)
    {
        const int SEARCH_LIMIT = 100;
        const int PLACE_SEARCH_LIMIT = 25;

        Direction[] directions = BoardDirection.directionList;

        var boardComponent = board.GetComponent<BoardWordSpy>();
        boardComponent.CleanBoard();

        List<string> selectedWords = new List<string>();

        foreach (int _ in Enumerable.Range(0, wordCount))
        {
            int searchLimitCnt = 0;
            do // Search `SEARCH_LIMIT` words and try `PLACE_SEARCH_LIMIT` attempt to place it
            {
                bool placed = false;
                ShuffleArray(directions);
                int placeLimitCnt = 0;

                string word; // If word is already selected, search for another word.
                do { word = WordDatabase.Instance.GetRandomWord(); } while (selectedWords.Contains(word));
                
                while (!placed && placeLimitCnt++ < PLACE_SEARCH_LIMIT)
                {
                    foreach (Direction direction in directions)
                    {
                        if (boardComponent.SetWord(word, boardComponent.GenerateRandomLetterLocation(), direction))
                        // TODO: For long words maybe increase the weight of the randomness of side locations
                        {
                            selectedWords.Add(word);
                            placed = true;
                            break;
                        }
                    }
                }
                if (placed)
                    break;
                else
                    Debug.LogError($"Cannot place word `{word}`. Please retry.");

            } while (searchLimitCnt++ < SEARCH_LIMIT);
        }

        boardComponent.FillEmptyLetters();
        m_selectedWords = selectedWords;
        UpdateSelectedWordListeners(selectedWords);
    }

    private void UpdateSelectedWordListeners(List<string> words)
    {
        OnSelectedWords?.Invoke(this, new OnSelectedWordsEventArgs { selectedWords = words });
    }

    private static void ShuffleArray<T>(T[] array)
    {
        System.Random rng = new System.Random();
        array = array.OrderBy(x => rng.Next()).ToArray();
    }

    private List<string> m_selectedWords = new List<string>();
}
