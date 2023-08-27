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

    protected override void InitLevelImpl(Level level)
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

    private bool UpdateBoardWordsRandomly(int wordCount)
    {
        List<string> words = new List<string>();

        foreach (int _ in Enumerable.Range(0, wordCount))
        {
            string randomWord;
            int cnt = 0;
            do
            {
                cnt++;
                randomWord = WordDatabase.Instance.GetRandomWord();
            }
            while (randomWord.Length > 10 && cnt < 100);

            if (cnt == 100)
            {
                Debug.LogError($"Cannot select random words.");
                return false;
            }

            words.Add(randomWord);
        }

        UpdateSelectedWordListeners(words);

        return UpdateBoardWords(words);
    }

    private void UpdateSelectedWordListeners(List<string> words)
    {
        OnSelectedWords?.Invoke(this, new OnSelectedWordsEventArgs { selectedWords = words });
    }


    private bool UpdateBoardWords(List<string> words)
    {
        m_selectedWords = words;
        return UpdateBoardWords();
    }

    private bool UpdateBoardWords()
    {
        Direction[] directions = {
            Direction.Up,
            Direction.Down,
            Direction.Right,
            Direction.Left,
            Direction.UpperRight,
            Direction.UpperLeft,
            Direction.DownRight,
            Direction.DownLeft
        };

        var boardComponent = board.GetComponent<BoardWordSpy>();
        boardComponent.CleanBoard();

        foreach (string word in m_selectedWords)
        {
            ShuffleArray(directions);
            bool placed = false;
            int cnt = 0;
            while (!placed && cnt < 25)
            {
                cnt++;
                foreach (Direction direction in directions)
                {
                    if (boardComponent.SetWord(word, boardComponent.GenerateRandomLetterLocation(), direction))
                    {
                        placed = true;
                        break;
                    }
                }
            }
            if (!placed)
            {
                Debug.LogError($"Cannot place new words. Please retry.");
                return false;
            }
        }

        boardComponent.FillEmptyLetters();

        return true;
    }

    private static void ShuffleArray<T>(T[] array)
    {
        System.Random rng = new System.Random();
        array = array.OrderBy(x => rng.Next()).ToArray();
    }

    private List<string> m_selectedWords = new List<string>();
}
