using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCubeVisual : MonoBehaviour
{
    [SerializeField] private LetterCube letterCube;

    private Material material;
    private Color startColor;

    private void Start()
    {
        material = transform.GetComponent<Renderer>().material;
        startColor = material.color;

        LetterSelectionChecker.Instance.OnLetterSelected += LetterSelectionChecker_OnLetterSelected;
        LetterSelectionChecker.Instance.OnLetterSelectable += LetterSelectionChecker_OnLetterSelectable;
        GameInput.Instance.OnSelectReleaseAction += GameInput_OnSelectReleaseAction;
    }

    private void LetterSelectionChecker_OnLetterSelected(object sender, LetterSelectionChecker.OnLetterSelectedEventArgs e)
    {
        if (letterCube == e.letterCube)
        {
            // what to do on select
            material.color = Color.blue;
        }
    }

    private void LetterSelectionChecker_OnLetterSelectable(object sender, LetterSelectionChecker.OnLetterSelectableEventArgs e)
    {
        if (letterCube == e.letterCube)
        {
            // what to do on select
            material.color = Color.grey;
        }
    }


    private void GameInput_OnSelectReleaseAction(object sender, EventArgs e)
    {
        material.color = startColor;
    }
}
