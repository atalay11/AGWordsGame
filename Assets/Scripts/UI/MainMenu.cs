using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    const string mainMenuScene = "MainMenuScene";
    const string playScene = "PlayScene";

    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(playScene, LoadSceneMode.Single);
    }

}
