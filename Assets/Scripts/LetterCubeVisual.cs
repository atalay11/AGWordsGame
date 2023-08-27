using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCubeVisual : MonoBehaviour
{
    private readonly string OnSelect = "OnSelected";

    [SerializeField] private LetterCube letterCube;

    private Material material;
    private Color startColor;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        material = transform.GetComponent<Renderer>().material;
        startColor = material.color;

        LetterSelectionChecker.Instance.OnLetterSelected += LetterSelectionChecker_OnLetterSelected;
        LetterSelectionChecker.Instance.OnLetterUnSelected += LetterSelectionChecker_OnLetterUnSelected;

        GameInput.Instance.OnSelectReleaseAction += GameInput_OnSelectReleaseAction;
    }

    private void LetterSelectionChecker_OnLetterSelected(object sender, LetterSelectionChecker.OnLetterSelectedEventArgs e)
    {
        if (letterCube == e.letterCube)
        {
            // what to do on select
            if (material.color != Color.blue)
                PlayOnSelectedAnimation();

            material.color = Color.blue;
        }
    }

    private void LetterSelectionChecker_OnLetterUnSelected(object sender, LetterSelectionChecker.OnLetterUnSelectedEventArgs e)
    {
        if (letterCube == e.letterCube)
        {
            // what to do on select
            material.color = startColor;
        }
    }


    private void GameInput_OnSelectReleaseAction(object sender, EventArgs e)
    {
        material.color = startColor;
    }

    private void PlayOnSelectedAnimation()
    {
        // var isIdle = animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        // var isLetterCubeVisual = animator.GetCurrentAnimatorStateInfo(0).IsName("LetterCubeVisual");
        // Debug.Log($"isIdle: {isIdle}");
        // Debug.Log($"isLetterCubeVisual: {isLetterCubeVisual}");


        animator.SetTrigger(OnSelect);
    }
}
