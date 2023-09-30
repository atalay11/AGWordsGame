using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSelectionChecker : GenericSingleton<LetterSelectionChecker>
{
    // Events
    public EventHandler<OnLetterSelectedEventArgs> OnLetterSelected;

    public class OnLetterSelectedEventArgs : EventArgs
    {
        public LetterCube letterCube;
    }

    public EventHandler<OnLetterUnSelectedEventArgs> OnLetterUnSelected;
    public class OnLetterUnSelectedEventArgs : EventArgs
    {
        public LetterCube letterCube;
    }

    public EventHandler<OnWordSelectedEventArgs> OnWordSelected;

    public class OnWordSelectedEventArgs : EventArgs
    {
        public string word;
        public LetterCube firstLetterCube;
        public Direction direction;
    }

    // variables

    LetterCube originLetterCube; // first selected cube
    List<LetterCube> selectedLetterCubes;
    List<LetterCube> currentLetterCubes;
    Direction wordDirection = Direction.Unknown;
    [SerializeField] LevelManagerWordSpyBase levelManagerWordSpyBase;

    // functions

    protected override void AwakeImpl()
    {
        selectedLetterCubes = new List<LetterCube>();
        currentLetterCubes = new List<LetterCube>();
    }

    private void Start()
    {
        GameInputWordSpy.Instance.OnLetterLayerSelectAction += GameInput_OnLetterLayerSelectAction;
        GameInputWordSpy.Instance.OnSelectReleaseAction += GameInput_OnSelectReleaseAction;
        levelManagerWordSpyBase.OnSelectedCorrect += LevelManagerWordSpy_OnSelectedCorrect;
    }

    private void GameInput_OnLetterLayerSelectAction(object sender, GameInputWordSpy.OnLetterLayerSelectEventArgs e)
    {
        if (originLetterCube == null)
        {
            originLetterCube = e.letterCube;
            OnLetterSelected?.Invoke(this, new OnLetterSelectedEventArgs { letterCube = e.letterCube });
        }

        if (e.letterCube == originLetterCube)
            return;

        var origin = originLetterCube.transform.position;
        var dest = e.letterCube.transform.position;
        Vector2 playerDirection = (dest - origin);

        Direction bestDirection = Direction.Unknown;
        Vector2 bestDirectionVector = Vector2.zero;
        float bestProduct = 0; // Actually distance
        foreach (var entry in BoardDirection.directionMappings)
        {
            Vector2 normalizedDirectionVector = entry.Value;
            normalizedDirectionVector = normalizedDirectionVector.normalized;
            var dotProduct = Vector2.Dot(normalizedDirectionVector, playerDirection);
            if (dotProduct > bestProduct)
            {
                bestDirection = entry.Key;
                bestDirectionVector = normalizedDirectionVector;
                bestProduct = dotProduct;
            }
        }

        var hits = Physics.RaycastAll(origin, bestDirectionVector, bestProduct);
        System.Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent<LetterCube>(out LetterCube letterCube))
            {
                var curDistance = Vector2.Distance(origin, letterCube.transform.position);
                if (curDistance <= bestProduct)
                {
                    currentLetterCubes.Add(letterCube);
                    OnLetterSelected?.Invoke(this, new OnLetterSelectedEventArgs { letterCube = letterCube });
                }
            }
        }

        foreach (var selectedLetterCube in selectedLetterCubes)
        {
            if (!currentLetterCubes.Contains(selectedLetterCube))
            {
                OnLetterUnSelected?.Invoke(this, new OnLetterUnSelectedEventArgs { letterCube = selectedLetterCube });
            }
        }

        selectedLetterCubes = new List<LetterCube>(currentLetterCubes);
        currentLetterCubes.Clear();
        wordDirection = bestDirection;
    }

    private void GameInput_OnSelectReleaseAction(object sender, EventArgs e)
    {
        if (originLetterCube != null)
        {
            string selectedWord = originLetterCube.GetLetter().ToString();
            foreach (var selectedLetterCube in selectedLetterCubes)
            {
                selectedWord += selectedLetterCube.GetLetter();
            }
            OnWordSelected?.Invoke(this, new OnWordSelectedEventArgs { word = selectedWord, firstLetterCube = originLetterCube, direction = wordDirection });
        }

        originLetterCube = null;
        selectedLetterCubes.Clear();
    }

    private void LevelManagerWordSpy_OnSelectedCorrect(object sender, EventArgs e)
    {
        originLetterCube.GetComponentInChildren<LetterCubeVisual>().PlayCorrectAnimation();
        foreach (var selected in selectedLetterCubes)
        {
            selected.GetComponentInChildren<LetterCubeVisual>().PlayCorrectAnimation();
        }
    }

}
