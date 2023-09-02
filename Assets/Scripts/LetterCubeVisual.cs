using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class LetterCubeVisual : MonoBehaviour
{
    private readonly string OnSelect = "OnSelected";
    private readonly string IsSelect = "IsSelected";

    [SerializeField] private LetterCube letterCube;
    [SerializeField] private Color selectColor;

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

        GameInputWordSpy.Instance.OnSelectReleaseAction += GameInput_OnSelectReleaseAction;
    }

    private void LetterSelectionChecker_OnLetterSelected(object sender, LetterSelectionChecker.OnLetterSelectedEventArgs e)
    {
        if (letterCube == e.letterCube)
        {
            // what to do on select
            if (material.color != selectColor)
                PlayOnSelectedAnimation();

            material.color = selectColor;


            PlayIsSelectedAnimation(true);
        }
    }

    private void LetterSelectionChecker_OnLetterUnSelected(object sender, LetterSelectionChecker.OnLetterUnSelectedEventArgs e)
    {
        if (letterCube == e.letterCube)
        {
            // what to do on select
            material.color = startColor;
            PlayIsSelectedAnimation(false);
        }
    }


    private void GameInput_OnSelectReleaseAction(object sender, EventArgs e)
    {
        material.color = startColor;
        PlayIsSelectedAnimation(false);
    }

    private void PlayOnSelectedAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(OnSelect);
        }
    }

    private void PlayIsSelectedAnimation(bool play)
    {
        if (animator != null)
        {
            animator.SetBool(IsSelect, play);
        }
    }
}
