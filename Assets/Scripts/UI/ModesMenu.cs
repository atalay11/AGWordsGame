using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModesMenu : GenericSingleton<ModesMenu>
{
    public void OnPlayButtonPressed()
    {
        SceneLoader.Instance.LoadLevelSelectionScene();
    }

    public void OnArcadeWordSpyButtonPressed()
    {
        SceneLoader.Instance.LoadGameScene(GameMode.WordSpyArcade);
    }

    public void OnBackIconButtonPressed()
    {
        Debug.Log("Load Main Menu Scene");
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
