using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    // Singleton

    public static GameInput Instance { get; private set; }

    // Events

    public EventHandler OnSelectReleaseAction;

    public EventHandler<OnLetterSelectEventArgs> OnLetterSelectAction;
    public class OnLetterSelectEventArgs : EventArgs
    {
        public LetterCube letterCube;
    }


    // Functions

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameInput is singleton but tried to be set more than once.");
        }

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Player Input Actions 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"Hit something {hit.transform.name}");
                if (hit.transform.TryGetComponent<LetterCube>(out LetterCube letterCube))
                {
                    OnLetterSelectAction?.Invoke(this, new OnLetterSelectEventArgs { letterCube = letterCube });
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnSelectReleaseAction?.Invoke(this, EventArgs.Empty);
        }
    }


    // Singleton





}
