using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class PlayModesSceneData : GenericSingleton<PlayModesSceneData>
{
    public GameMode GameMode{
        get { return m_GameMode; }
        set { m_GameMode = value; }   
    }

    private GameMode m_GameMode = GameMode.WordSpyGrandpa;
}
