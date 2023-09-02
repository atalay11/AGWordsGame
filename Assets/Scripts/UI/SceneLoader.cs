using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    const string mainMenuScene = "MainMenuScene";
    const string wordSpyScene = "WordSpyScene";
    const string playModesScene = "PlayModesScene";
    const string levelSelectionScene = "LevelSelectionScene";

    static public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
    }

    static public void LoadGameScene(GameMode gameMode)
    {
        if ( gameMode == GameMode.WordSpyGrandpa )
        {
            SceneManager.LoadScene(wordSpyScene, LoadSceneMode.Single);
        }
        else // TODO: Replace this
        {
            SceneManager.LoadScene(wordSpyScene, LoadSceneMode.Single);
        }
        // else if ( gameMode == GameMode.WordSpyArcade )
        // {
        //     // TODO:
        // }
        // else if ( gameMode == GameMode.WordSpyBlindfold )
        // {
        //     // TODO:
        // }
        // else if ( gameMode == GameMode.CrossWordGrandpa )
        // {
        //     // TODO:
        // }
    }

    static public void LoadPlayModesScene()
    {
        SceneManager.LoadScene(playModesScene, LoadSceneMode.Single);
    }
    
    static public void LoadLevelSelectionScene()
    {
        SceneManager.LoadScene(levelSelectionScene, LoadSceneMode.Single);
    }

}
