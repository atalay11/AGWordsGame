using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject letterSelectionChecker;
    [SerializeField] private List<string> selectedWords = new List<string>();
    private System.Random _random = new System.Random();
    public void NewBoard(int edgeLength)
    {
        board.GetComponent<Board>().ResetWithNewEdgeLength(edgeLength);
    }

    private void Start()
    {
        letterSelectionChecker.GetComponent<LetterSelectionChecker>().OnWordSelected += LetterSelectionChecker_OnWordSelected;
        UpdateBoardWordsRandomly(3);
    }

    private void LetterSelectionChecker_OnWordSelected(object sender, LetterSelectionChecker.OnWordSelectedEventArgs e)
    {
        if (e.direction == Direction.Unknown)
            return;
        // Extra check, this might be unnecessary
        bool overlaps = board.GetComponent<Board>().DoesWordOverlap(e.word, e.firstLetterCube, e.direction);
    }

    private bool UpdateBoardWordsRandomly(int wordCount = 8)
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

        return UpdateBoardWords(words);
    }


    private bool UpdateBoardWords(List<string> words)
    {
        selectedWords = words;
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
        var board_comp = board.GetComponent<Board>();
        board_comp.CleanBoard();

        foreach (string word in selectedWords)
        {
            Shuffle(directions);
            bool placed = false;
            int cnt = 0;
            while (!placed && cnt < 25)
            {
                cnt++;
                foreach (Direction direction in directions)
                {
                    if (board_comp.SetWord(word, board_comp.GenerateRandomLetterLocation(), direction))
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

        board_comp.FillEmptyLetters();

        return true;
    }

    private void Shuffle(Direction[] array)
    {
        int p = array.Length;
        for (int n = p - 1; n > 0; n--)
        {
            int r = _random.Next(0, n);
            Direction t = array[r];
            array[r] = array[n];
            array[n] = t;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            NewBoard(10);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            NewBoard(15);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            NewBoard(20);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            for (int numberOfWords = 8; !UpdateBoardWordsRandomly(numberOfWords); numberOfWords--) { }
        }
    }
}
