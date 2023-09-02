using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeButtonClickHandler : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;
    public void OnButtonClick()
    {
        PlayModesSceneData.Instance.GameMode = gameMode;
        ModesMenu.Instance.OnPlayButtonPressed();
    }
}
