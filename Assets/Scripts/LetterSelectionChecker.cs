using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSelectionChecker : MonoBehaviour
{
    // Singleton
    public static LetterSelectionChecker Instance { get; private set; }

    // Events
    public EventHandler<OnLetterSelectedEventArgs> OnLetterSelected;

    public class OnLetterSelectedEventArgs : EventArgs
    {
        public LetterCube letterCube;
    }


    public EventHandler<OnLetterSelectableEventArgs> OnLetterSelectable;

    public class OnLetterSelectableEventArgs : EventArgs
    {
        public LetterCube letterCube;
    }

    // variables

    LetterCube originLetterCube; // first selected cube
    LetterCube directionLetterCube; // second selected cube
    List<LetterCube> selectableLetterCubes;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameInput is singleton but tried to be set more than once.");
        }

        Instance = this;

        selectableLetterCubes = new List<LetterCube>();
    }

    private void Start()
    {

        GameInput.Instance.OnLetterLayerSelectAction += GameInput_OnLetterLayerSelectAction;
        GameInput.Instance.OnSelectReleaseAction += GameInput_OnSelectReleaseAction;
    }

    private void GameInput_OnLetterLayerSelectAction(object sender, GameInput.OnLetterLayerSelectEventArgs e)
    {
        if (originLetterCube == null)
        {
            Debug.Log("Setting original");
            originLetterCube = e.letterCube;
            OnLetterSelected?.Invoke(this, new OnLetterSelectedEventArgs { letterCube = e.letterCube });
        }

        if (directionLetterCube == null)
        {
            if (e.letterCube == originLetterCube)
                return;

            Debug.Log("Setting directional");
            directionLetterCube = e.letterCube;
            OnLetterSelected?.Invoke(this, new OnLetterSelectedEventArgs { letterCube = directionLetterCube });

            // set direction and selectable cubes
            var origin = originLetterCube.transform.position;
            var dest = directionLetterCube.transform.position;
            var direction = (dest - origin).normalized;

            const float maxHitDist = 100f;
            var hits = Physics.RaycastAll(dest, direction, maxHitDist);

            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent<LetterCube>(out LetterCube letterCube))
                {
                    selectableLetterCubes.Add(letterCube);
                    OnLetterSelectable?.Invoke(this, new OnLetterSelectableEventArgs { letterCube = letterCube });
                }
            }
        }

        if (e.letterCube == originLetterCube ||
            e.letterCube == directionLetterCube ||
            selectableLetterCubes.Contains(e.letterCube)
        )
        {
            var origin = originLetterCube.transform.position;

            // if distance is between this range it is selected
            var curSelectedPos = e.letterCube.transform.position;
            var selectDistance = Vector3.Distance(origin, curSelectedPos);

            foreach (var selectableLetterCube in selectableLetterCubes)
            {
                var curDistance = Vector3.Distance(origin, selectableLetterCube.transform.position);

                if (curDistance <= selectDistance)
                {
                    // here is the selected
                    // Debug.Log($"selected: {letterCube.GetLetter()}");
                    OnLetterSelected?.Invoke(this, new OnLetterSelectedEventArgs { letterCube = selectableLetterCube });

                }
                else
                {
                    // here is the selectable
                    // Debug.Log($"selectable: {letterCube.GetLetter()}");
                    OnLetterSelectable?.Invoke(this, new OnLetterSelectableEventArgs { letterCube = selectableLetterCube });
                }
            }
        }
    }

    private void GameInput_OnSelectReleaseAction(object sender, EventArgs e)
    {
        originLetterCube = null;
        directionLetterCube = null;
        selectableLetterCubes.Clear();
    }
}
