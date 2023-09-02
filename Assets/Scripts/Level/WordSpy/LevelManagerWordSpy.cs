using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelManagerWordSpy : LevelManagerBase
{
    [SerializeField] private GameObject board;
    [SerializeField] private int currentEdgeLength;

    public EventHandler<OnSelectedWordsEventArgs> OnSelectedWords;
    public class OnSelectedWordsEventArgs : EventArgs
    {
        public List<string> selectedWords;
    }

    public EventHandler<OnLevelUpEventArgs> OnLevelUp; //? TODO: First level 
    public class OnLevelUpEventArgs: EventArgs
    {
        public Level level;
    }
    
    private void Awake()
    {
        m_CurrentLevel = LevelSelectionSceneData.Instance.LevelInfo;
        SetLevel(m_CurrentLevel);
    }

    private void Start()
    {
        LetterSelectionChecker.Instance.OnWordSelected += LevelManagerWordSpy_OnWordSelected;
    }

    private void LevelManagerWordSpy_OnWordSelected(object sender, LetterSelectionChecker.OnWordSelectedEventArgs e)
    {
        if(m_SelectedWords.Contains(e.word))
        {
            m_SelectedWords.Remove(e.word);
        }

        if (m_SelectedWords.Count() == 0)
        {
            NextLevel();
        }
    }

    public void NewBoard(int edgeLength)
    {
        currentEdgeLength = edgeLength;
        board.GetComponent<BoardWordSpy>().ResetWithNewEdgeLength(currentEdgeLength);
    }

    private void SetLevel(Level level)
    {
        if (m_LevelData.TryGetValue(level, out LevelInfo levelData))
        {
            NewBoard(levelData.edgeLength);
            UpdateBoardWordsRandomly(levelData.wordCount);
        }
        else
        {
            NewBoard(25); // TODO: Max level
            UpdateBoardWordsRandomly(16);
        }
        m_OldLevelWords.UnionWith(m_SelectedWords);
    }

    private void NextLevel()
    {
        m_CurrentLevel = Level.GetNextLevel(m_CurrentLevel); // TODO: NextLevelEvent
        SetLevel(m_CurrentLevel);
        RaiseOnLevelUpWord();
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

    private void RaiseOnLevelUpWord()
    {
        OnLevelUp?.Invoke(this, new OnLevelUpEventArgs { level=m_CurrentLevel });
    }

    private static void ShuffleArray<T>(T[] array)
    {
        System.Random rng = new System.Random();
        array = array.OrderBy(x => rng.Next()).ToArray();
    }

    private List<string> m_SelectedWords = new List<string>();
    private HashSet<string> m_OldLevelWords = new HashSet<string>();
    private Level m_CurrentLevel = new Level(1);

    private class LevelInfo
    {
        public int edgeLength;
        public int wordCount;
    }
    private Dictionary<Level, LevelInfo> m_LevelData = new Dictionary<Level, LevelInfo>
        {
            { new Level(1),  new LevelInfo { edgeLength = 5,  wordCount = 1  } },
            { new Level(2),  new LevelInfo { edgeLength = 6,  wordCount = 1  } },
            { new Level(3),  new LevelInfo { edgeLength = 7,  wordCount = 2  } },
            { new Level(4),  new LevelInfo { edgeLength = 8,  wordCount = 2  } },
            { new Level(5),  new LevelInfo { edgeLength = 9,  wordCount = 3  } },
            { new Level(6),  new LevelInfo { edgeLength = 10, wordCount = 4  } },
            { new Level(7),  new LevelInfo { edgeLength = 11, wordCount = 5  } },
            { new Level(8),  new LevelInfo { edgeLength = 12, wordCount = 5  } },
            { new Level(9),  new LevelInfo { edgeLength = 13, wordCount = 5  } },
            { new Level(10), new LevelInfo { edgeLength = 14, wordCount = 6  } },
            { new Level(11), new LevelInfo { edgeLength = 15, wordCount = 6  } },
            { new Level(12), new LevelInfo { edgeLength = 16, wordCount = 6  } },
            { new Level(13), new LevelInfo { edgeLength = 17, wordCount = 7  } },
            { new Level(14), new LevelInfo { edgeLength = 18, wordCount = 8  } },
            { new Level(15), new LevelInfo { edgeLength = 19, wordCount = 9  } },
            { new Level(16), new LevelInfo { edgeLength = 20, wordCount = 10 } },
            { new Level(17), new LevelInfo { edgeLength = 21, wordCount = 12 } },
            { new Level(18), new LevelInfo { edgeLength = 22, wordCount = 13 } },
            { new Level(19), new LevelInfo { edgeLength = 23, wordCount = 14 } },
            { new Level(20), new LevelInfo { edgeLength = 24, wordCount = 15 } }
        };
}



