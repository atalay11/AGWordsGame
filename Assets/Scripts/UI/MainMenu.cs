using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : GenericSingleton<MainMenu>
{
    public void OnPlayButtonPressed()
    {
        SceneLoader.Instance.LoadPlayModesScene();
    }

    public void OnSettingsButtonPressed()
    {
        Debug.Log("OnSettingsButtonPressed");
        mainMenuVisual.gameObject.SetActive(false);
        settingsMenuVisual.gameObject.SetActive(true);
    }

    public void OnQuitButtonPressed()
    {
        // Check if the application is running in the Unity Editor (for testing)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If not in the Unity Editor, quit the application
            Application.Quit();
#endif
    }

    // Settings Menu

    public void OnLanguageSelectButtonPressed()
    {
        Debug.Log("OnLanguageSelectButtonPressed");
        languageSelectionVisual.gameObject.SetActive(true);
    }

    public void OnSettingsMenuCloseButtonPressed()
    {
        Debug.Log("OnSettingsMenuClosed");
        mainMenuVisual.gameObject.SetActive(true);
        settingsMenuVisual.gameObject.SetActive(false);
    }

    public void OnLanguageSelected()
    {
        Debug.Log("OnLanguageSelected");
        languageSelectionVisual.gameObject.SetActive(false);
    }

    // Variables

    [SerializeField] private Transform mainMenuVisual;
    [SerializeField] private Transform settingsMenuVisual;
    [SerializeField] private Transform languageSelectionVisual;


}
