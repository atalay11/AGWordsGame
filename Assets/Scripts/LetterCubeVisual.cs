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

        GameInput.Instance.OnLetterSelectAction += GameInput_OnLetterSelectAction;
        GameInput.Instance.OnSelectReleaseAction += GameInput_OnSelectReleaseAction;
    }

    private void GameInput_OnLetterSelectAction(object sender, GameInput.OnLetterSelectEventArgs e)
    {
        if (letterCube == e.letterCube)
        {
            // what to do on select
            Debug.Log("Selamlar");
            material.color = Color.blue;
        }
    }

    private void GameInput_OnSelectReleaseAction(object sender, EventArgs e)
    {
        material.color = startColor;
    }
}
