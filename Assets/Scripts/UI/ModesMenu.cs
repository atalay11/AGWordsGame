using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModesMenu : MonoBehaviour
{
    public void OnPlayButtonPressed()
    {
        SceneLoader.LoadWordSpyScene();
    }

    public void OnBackIconButtonPressed()
    {
        Debug.Log("Load Main Menu Scene");
        SceneLoader.LoadMainMenuScene();
    }
}
