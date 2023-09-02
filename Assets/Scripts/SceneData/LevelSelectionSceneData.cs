using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionSceneData : GenericSingleton<LevelSelectionSceneData>
{
    public Level LevelInfo{
        get { return m_Level; }
        set { m_Level = value; }   
    }

    private Level m_Level = new Level(1);
}
