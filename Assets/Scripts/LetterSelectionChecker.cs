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

    public EventHandler<OnLetterSelectableEventArgs> OnLetterSelectable;

    public class OnLetterSelectableEventArgs : EventArgs
    {
        public LetterCube letterCube;
    }

    public EventHandler<OnSelectedWordChangedEventArgs> OnSelectedWordChanged;

    public class OnSelectedWordChangedEventArgs : EventArgs
    {
        public string word;
    }

    // variables

    LetterCube originLetterCube; // first selected cube
    LetterCube directionLetterCube; // second selected cube
    List<LetterCube> selectableLetterCubes; // * We should remove this
    List<LetterCube> selectedLetterCubes;
    List<LetterCube> currentLetterCubes;

    Vector3[] mainDirections = {
        new Vector3(0 , 1, 0).normalized,
        new Vector3(0 ,-1, 0).normalized,
        new Vector3(1 , 0, 0).normalized,
        new Vector3(-1, 0, 0).normalized,
        new Vector3(1 , 1, 0).normalized,
        new Vector3(-1,-1, 0).normalized,
        new Vector3(1 ,-1, 0).normalized,
        new Vector3(-1, 1, 0).normalized,
    };

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameInput is singleton but tried to be set more than once.");
        }

        Instance = this;

        selectableLetterCubes = new List<LetterCube>();
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
        var playerDirection = (dest - origin);

        Vector3 bestDirection = Vector3.zero;
        float bestProduct = 0; // Actually distance
        foreach (var direction in mainDirections){
            var dotProduct = Vector3.Dot(direction, playerDirection);
            if(dotProduct > bestProduct){
                bestDirection = direction;
                bestProduct = dotProduct;
            }
        }
        
        const float maxHitDist = 100f;
        var hits = Physics.RaycastAll(origin, bestDirection, maxHitDist);            
        System.Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));
        
        foreach (var hit in hits)
        {
            if(hit.transform.TryGetComponent<LetterCube>(out LetterCube letterCube)){
                var curDistance = Vector3.Distance(origin, letterCube.transform.position);
                if(curDistance <= bestProduct){
                    currentLetterCubes.Add(letterCube);
                    OnLetterSelected?.Invoke(this, new OnLetterSelectedEventArgs { letterCube = letterCube });
                }
            }
        }

        foreach (var selectedLetterCube in selectedLetterCubes){
            if(!currentLetterCubes.Contains(selectedLetterCube)){
                OnLetterUnSelected?.Invoke(this, new OnLetterUnSelectedEventArgs { letterCube = selectedLetterCube });
            }
        }
        
        selectedLetterCubes = new List<LetterCube>(currentLetterCubes);
        currentLetterCubes.Clear();
    }

    private void GameInput_OnSelectReleaseAction(object sender, EventArgs e)
    {
        if(originLetterCube != null){
            string selectedWord = originLetterCube.GetLetter().ToString();
            foreach (var selectedLetterCube in selectedLetterCubes){
                selectedWord += selectedLetterCube.GetLetter();
            }
            // ! not OnSelectedWordChangedEventArgs because release action send word to dictionary
            OnSelectedWordChanged?.Invoke(this, new OnSelectedWordChangedEventArgs { word = selectedWord });
        }
        
        originLetterCube = null;
        selectableLetterCubes.Clear();
        selectedLetterCubes.Clear();
    }
}
