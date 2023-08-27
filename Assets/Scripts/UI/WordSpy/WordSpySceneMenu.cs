using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneMenu : MonoBehaviour
{

    public void OnBackIconButtonPressed()
    {
        Debug.Log("LoadPlayModesScene");
        SceneLoader.LoadPlayModesScene();
    }


}
