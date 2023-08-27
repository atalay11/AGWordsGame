using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputWordSpy : GenericSingleton<GameInputWordSpy>
{
    // Events

    public EventHandler OnSelectReleaseAction;

    public EventHandler<OnLetterLayerSelectEventArgs> OnLetterLayerSelectAction;
    public class OnLetterLayerSelectEventArgs : EventArgs
    {
        public LetterCube letterCube;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Player Input Actions 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // const int letterLayer = 6;
            // const float maxDist = 10f;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent<LetterCube>(out LetterCube letterCube))
                {
                    OnLetterLayerSelectAction?.Invoke(this, new OnLetterLayerSelectEventArgs { letterCube = letterCube });
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnSelectReleaseAction?.Invoke(this, EventArgs.Empty);
        }
    }
}
