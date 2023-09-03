using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneMenu : MonoBehaviour
{

    public void OnBackIconButtonPressed()
    {
        Debug.Log("LoadLevelSelectionScene");
        SceneLoader.LoadLevelSelectionScene();
    }

    // Bunlar hep ömer yüzünden
    public void GoToModeSelection()
    {
        SceneLoader.LoadPlayModesScene();
    }


}
