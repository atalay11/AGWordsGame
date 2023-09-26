using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using DG.Tweening;
using System.Text;

public class LetterCubeVisual : MonoBehaviour
{
    private enum State
    {
        Normal,
        Select,
        Correct,
        CorrectSelect,
    }

    private readonly string OnSelect = "OnSelected";
    private readonly string IsSelect = "IsSelected";
    private const float colorChangeTime = 0.75f;

    [SerializeField] private LetterCube letterCube;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectColor;
    [SerializeField] private Color correctColor;


    private State m_state;

    private Material material;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        material = transform.GetComponent<Renderer>().material;
        material.color = normalColor;

        LetterSelectionChecker.Instance.OnLetterSelected += LetterSelectionChecker_OnLetterSelected;
        LetterSelectionChecker.Instance.OnLetterUnSelected += LetterSelectionChecker_OnLetterUnSelected;

        GameInputWordSpy.Instance.OnSelectReleaseAction += GameInput_OnSelectReleaseAction;
    }

    private void LetterSelectionChecker_OnLetterSelected(object sender, LetterSelectionChecker.OnLetterSelectedEventArgs e)
    {
        if (letterCube == e.letterCube)
        {
            if (m_state == State.Select || m_state == State.CorrectSelect)
                return;

            // what to do on select
            PlayOnSelectedAnimation();

            SetState(State.Select);
            SetStateColor();


            PlayIsSelectedAnimation(true);
            // SoundManager.PlaySound(SoundManager.Sound.SelectLetterCube);
        }
    }

    private void LetterSelectionChecker_OnLetterUnSelected(object sender, LetterSelectionChecker.OnLetterUnSelectedEventArgs e)
    {
        if (letterCube == e.letterCube)
        {
            // what to do on select
            SetState(State.Normal);
            SetStateColor();
            PlayIsSelectedAnimation(false);
        }
    }

    private void GameInput_OnSelectReleaseAction(object sender, EventArgs e)
    {
        // material.color = startColor;
        SetState(State.Normal);
        SetStateColor();
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

    public void PlayCorrectAnimation()
    {
        SetState(State.Correct);
    }

    private void SetState(State state)
    {
        if (m_state == State.CorrectSelect && state == State.Normal)
            m_state = State.Correct;
        if (m_state == State.CorrectSelect && state == State.Select)
            m_state = State.CorrectSelect;
        else if (m_state == State.Correct && state == State.Normal)
            m_state = State.Correct;
        else if (m_state == State.Correct && state == State.Select)
            m_state = State.CorrectSelect;
        else
            m_state = state;
    }

    private void SetStateColor()
    {
        switch (m_state)
        {
            case State.Normal:
                material.DOColor(normalColor, colorChangeTime);

                break;
            case State.Select:
            case State.CorrectSelect:
                material.DOColor(selectColor, colorChangeTime);
                break;
            case State.Correct:
                material.DOColor(correctColor, colorChangeTime);
                break;
        }
    }


}
