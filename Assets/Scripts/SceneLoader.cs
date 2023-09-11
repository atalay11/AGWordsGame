using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : GenericSingleton<SceneLoader>
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;

    const string mainMenuScene = "MainMenuScene";
    const string playModesScene = "PlayModesScene";
    const string levelSelectionScene = "LevelSelectionScene";

    const string timedWordSpyScene = "TimedWordSpyScene";
    const string grandPaWordSpyScene = "GrandPaWordSpyScene";

    public void LoadMainMenuScene()
    {
        LoadSceneAsyncWithLoadingScene(mainMenuScene, LoadSceneMode.Single);
    }

    public void LoadGameScene(GameMode gameMode)
    {
        string sceneName = grandPaWordSpyScene;

        if (gameMode == GameMode.WordSpyGrandpa)
            sceneName = grandPaWordSpyScene;
        else if (gameMode == GameMode.WordSpyArcade)
            sceneName = timedWordSpyScene;

        LoadSceneAsyncWithLoadingScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadPlayModesScene()
    {
        LoadSceneAsyncWithLoadingScene(playModesScene, LoadSceneMode.Single);
    }

    public void LoadLevelSelectionScene()
    {
        LoadSceneAsyncWithLoadingScene(levelSelectionScene, LoadSceneMode.Single);
    }

    private void LoadSceneAsyncWithLoadingScene(string sceneName, LoadSceneMode mode)
    {
        StartCoroutine(LoadSceneAsyncWithLoadingSceneImpl(sceneName, mode));
    }

    private IEnumerator LoadSceneAsyncWithLoadingSceneImpl(string sceneName, LoadSceneMode mode)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, mode);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            yield return null;
        }
        loadingScreen.SetActive(false);
    }

}
