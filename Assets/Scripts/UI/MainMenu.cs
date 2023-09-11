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
        //TODO: Load Settings Scene
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

}
