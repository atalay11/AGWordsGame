using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : GenericSingleton<SceneLoader>
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private float loadDummyWaitTime = 0.5f;

    const string mainMenuScene = "MainMenuScene";
    const string playModesScene = "PlayModesScene";
    const string levelSelectionScene = "LevelSelectionScene";

    const string timedWordSpyScene = "TimedWordSpyScene";
    const string grandPaWordSpyScene = "GrandPaWordSpyScene";

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
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
        SceneManager.LoadScene(playModesScene, LoadSceneMode.Single);
    }

    public void LoadLevelSelectionScene()
    {
        SceneManager.LoadScene(levelSelectionScene, LoadSceneMode.Single);
    }

    private void LoadSceneAsyncWithLoadingScene(string sceneName, LoadSceneMode mode)
    {
        StartCoroutine(LoadSceneAsyncWithLoadingSceneImpl(sceneName, mode));
    }

    private IEnumerator LoadSceneAsyncWithLoadingSceneImpl(string sceneName, LoadSceneMode mode)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, mode);

        loadingScreen.SetActive(true);

        float timer = 0.0f;
        while(!operation.isDone || timer < loadDummyWaitTime)
        {
            // float progress = Mathf.Clamp01(operation.progress / 0.9f);
            float progress = timer / loadDummyWaitTime;
            slider.value = progress;
            timer += Time.deltaTime;
            progressText.text = "Loading... " + (int)(progress * 100.0f) + "%";

            yield return null;
        }
        loadingScreen.SetActive(false);
        slider.value = 0.0f;
    }

}
