using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelManagerWordSpyBase : LevelManagerBase
{
    public class LevelInfo
    {
        public int edgeLength;
        public int wordCount;
    }

    // -- Functions -- 

    private void Start()
    {
        LetterSelectionChecker.Instance.OnWordSelected += LetterSelectionChecker_OnWordSelected;
    }

    public void NewBoard(LevelInfo levelInfo)
    {
        m_CurrentLevelInfo = levelInfo;

        board.GetComponent<BoardWordSpy>().ResetWithNewEdgeLength(m_CurrentLevelInfo.edgeLength);
        UpdateBoardWordsRandomly(m_CurrentLevelInfo.wordCount);
        m_OldLevelWords.UnionWith(m_SelectedWords);
    }

    private void UpdateBoardWordsRandomly(int wordCount)
    {
        const int SEARCH_LIMIT = 50;
        const int PLACE_SEARCH_LIMIT = 50;

        Direction[] directions = BoardDirection.directionList;

        var boardComponent = board.GetComponent<BoardWordSpy>();
        boardComponent.CleanBoard();

        List<string> selectedWords = new List<string>();

        foreach (int _ in Enumerable.Range(0, wordCount))
        {
            int searchLimitCnt = 0;
            do // Search `SEARCH_LIMIT` words and try `PLACE_SEARCH_LIMIT` attempt for each word to place it on the Board
            {
                bool placed = false;
                ShuffleArray(directions);
                int placeLimitCnt = 0;

                string word; // If word is already selected, search for another word.
                do { word = WordDatabase.Instance.GetRandomWord(); } while (selectedWords.Contains(word) || m_OldLevelWords.Contains(word));

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
                    Debug.LogError($"Cannot place word `{word}`.");

            } while (searchLimitCnt++ < SEARCH_LIMIT);
        }

        boardComponent.FillEmptyLetters();
        m_SelectedWords = selectedWords;
        RaiseOnSelectedWord(selectedWords);
    }

    private void RaiseOnSelectedWord(List<string> words)
    {
        OnSelectedWords?.Invoke(this, new OnSelectedWordsEventArgs { selectedWords = words });
    }

    private static void ShuffleArray<T>(T[] array)
    {
        System.Random rng = new System.Random();
        array = array.OrderBy(x => rng.Next()).ToArray();
    }

    public LevelInfo GetLevelInfo()
    {
        return m_CurrentLevelInfo;
    }

    public List<string> GetRemainingWords()
    {
        return m_SelectedWords;
    }

    private void LetterSelectionChecker_OnWordSelected(object sender, LetterSelectionChecker.OnWordSelectedEventArgs e)
    {
        if (m_SelectedWords.Contains(e.word))
        {
            m_SelectedWords.Remove(e.word);
            OnCorrectWordSelectedImpl(e.word);
        }
    }

    virtual protected void OnCorrectWordSelectedImpl(string word)
    {
    }

    // -- SerializeField Variables--

    [SerializeField] private GameObject board;
    [SerializeField] private int currentEdgeLength;

    // -- Variables --

    private List<string> m_SelectedWords = new List<string>();
    private HashSet<string> m_OldLevelWords = new HashSet<string>();
    private LevelInfo m_CurrentLevelInfo;

    // -- Events --

    public EventHandler<OnSelectedWordsEventArgs> OnSelectedWords;
    public class OnSelectedWordsEventArgs : EventArgs
    {
        public List<string> selectedWords;
    }

}



