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

    public EventHandler<OnLetterUnSelectedEventArgs> OnLetterUnSelected;
    public class OnLetterUnSelectedEventArgs : EventArgs
    {
        public LetterCube letterCube;
    }

    public EventHandler<OnSelectedWordChangedEventArgs> OnSelectedWordChanged;

    public class OnSelectedWordChangedEventArgs : EventArgs
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

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameInput is singleton but tried to be set more than once.");
        }

        Instance = this;

        selectedLetterCubes = new List<LetterCube>();
        currentLetterCubes = new List<LetterCube>();
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
            originLetterCube = e.letterCube;
            OnLetterSelected?.Invoke(this, new OnLetterSelectedEventArgs { letterCube = e.letterCube });
        }

        if (e.letterCube == originLetterCube)
            return;

        var origin = originLetterCube.transform.position;
        var dest = e.letterCube.transform.position;
        Vector2 playerDirection = (dest - origin);

        Direction bestDirection = Direction.Unknown; // Add Direction some Unknown Direction
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
            // ! not OnSelectedWordChangedEventArgs because release action send word to dictionary
            OnSelectedWordChanged?.Invoke(this, new OnSelectedWordChangedEventArgs { word = selectedWord, firstLetterCube = originLetterCube, direction = wordDirection });
        }

        originLetterCube = null;
        selectedLetterCubes.Clear();
    }
}
