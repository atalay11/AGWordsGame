using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonClickHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    public void OnButtonClick()
    {
        var levelStr = textMesh.text;

        if (int.TryParse(levelStr, out int level))
        {
            LevelSelectionSceneData.Instance.LevelInfo = new Level(level);
        }
        else
        {
            Debug.LogError($"Level string could not be converted into int. levelStr: {levelStr}");
        }

        LevelSelectionMenu.Instance.OnLevelButtonPressed();
    }
}
