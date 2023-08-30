using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    const string mainMenuScene = "MainMenuScene";
    const string wordSpyScene = "WordSpyScene";
    const string playModesScene = "PlayModesScene";

    static public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
    }
    static public void LoadWordSpyScene()
    {
        SceneManager.LoadScene(wordSpyScene, LoadSceneMode.Single);
    }
    static public void LoadPlayModesScene()
    {
        SceneManager.LoadScene(playModesScene, LoadSceneMode.Single);
    }

}
