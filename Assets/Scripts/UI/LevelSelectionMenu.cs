using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelSelectionMenu : GenericSingleton<LevelSelectionMenu>
{
    [SerializeField] private LevelButtonGenerator levelButtonGenerator;

    protected override void AwakeImpl()
    {
        m_GameMode = PlayModesSceneData.Instance.GameMode;
        InstantiateLevelButtonsForGameMode();
    }

    public void OnLevelButtonPressed()
    {
        SceneLoader.Instance.LoadGameScene(m_GameMode);
    }

    public void OnBackIconButtonPressed()
    {
        Debug.Log("Load Game Modes Scene");
        SceneLoader.Instance.LoadPlayModesScene();
    }

    private void InstantiateLevelButtonsForGameMode()
    {
        if (m_GameMode == GameMode.WordSpyGrandpa)
        {
            Debug.Log("WordSpyGrandpa level buttons");
            foreach (int level in Enumerable.Range(1, 26))
            {
                levelButtonGenerator.Generate(new Level(level));
            }
        }
        else
        {
            Debug.Log("Other Game Modes level buttons");
        }
    }

    private GameMode m_GameMode = GameMode.WordSpyGrandpa;
}

