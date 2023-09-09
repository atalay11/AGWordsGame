using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class GrandPaLevelManagerWordSpy : LevelManagerWordSpyBase
{
    protected override void StartImpl()
    {
        m_CurrentLevel = LevelSelectionSceneData.Instance.LevelInfo;
        SetLevel(m_CurrentLevel);
    }

    private void SetLevel(Level level)
    {
        if (m_LevelData.TryGetValue(level, out LevelInfo levelData))
        {
            NewBoard(levelData);
        }
        else
        {
            LevelInfo maxLevel = new LevelInfo { edgeLength = 25, wordCount = 16 };
            NewBoard(maxLevel);
        }
        m_OldLevelWords.UnionWith(GetRemainingWords());
    }

    private void NextLevel()
    {
        m_CurrentLevel = Level.GetNextLevel(m_CurrentLevel); // TODO: NextLevelEvent
        SetLevel(m_CurrentLevel);
        RaiseOnLevelUpWord();
    }

    private void RaiseOnLevelUpWord()
    {
        OnLevelUp?.Invoke(this, new OnLevelUpEventArgs { level = m_CurrentLevel });
    }

    override protected void OnCorrectWordSelectedImpl(string word)
    {
        if (GetRemainingWords().Count == 0)
        {
            NextLevel();
        }
    }

    // --

    private HashSet<string> m_OldLevelWords = new HashSet<string>();
    private Level m_CurrentLevel = new Level(1);

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
            { new Level(20), new LevelInfo { edgeLength = 24, wordCount = 15 } },
        };


    public EventHandler<OnLevelUpEventArgs> OnLevelUp; //? TODO: First level 
    public class OnLevelUpEventArgs : EventArgs
    {
        public Level level;
    }


}



