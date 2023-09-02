using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelButtonGenerator : MonoBehaviour
{
    [SerializeField] private Transform levelButtonPrefab;
    [SerializeField] private Transform parentObject;

    public void Generate(Level level)
    {
        var newLevelButton = Instantiate(levelButtonPrefab);
        newLevelButton.parent = parentObject;
        newLevelButton.localScale = Vector3.one;
        var textMesh = newLevelButton.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = level.Value.ToString();
    }
}
