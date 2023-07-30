using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


public class LetterMap : MonoBehaviour
{
    [SerializeField] private LetterGenerator letterGenerator;
    [SerializeField] private int edgeLength = 10; // board size will be -> edgeLength - edgeLength (width - height)
    [SerializeField] private float spacing = 0.05f;

    // 0, 0 is bottom-left
    private Dictionary<LetterLocation, Transform> m_LetterMap;
    private float m_ElementScaling;
    private float m_ElementSpacing;

    // Return false if no such toLocation
    public bool SetLetter(Letter<char> letter, LetterLocation toLocation)
    {
        if (m_LetterMap.TryGetValue(toLocation, out Transform letterCubeTransform))
        {
            letterCubeTransform.GetComponent<LetterCube>().SetLetter(letter);
            return true;
        }
        return false;
    }

    public bool SetWord(string word, LetterLocation fromLocation, Direction toDirection)
    {
        if (string.IsNullOrEmpty(word))
        {
            Debug.LogWarning($"word is null or empty.");
            return false;
        }

        var directionVec = BoardDirection.GetVector(toDirection);
        if (!CheckSetWordInputsValid(fromLocation, directionVec, word.Length))
        {
            Debug.LogWarning($"SetWord inputs are invalid. fromLoc: `({fromLocation.column}, {fromLocation.row})`, word: `{word}`, directionVec: `{directionVec}`");
            return false;
        }

        var locationCursor = fromLocation;
        foreach (char ch in word)
        {
            bool success = SetLetter(new Letter<char>(ch), locationCursor);
            Assert.IsTrue(success, "All letters must be set successfully if this code block is executed. Check if CheckSetWordInputsValid function works properly.");
            locationCursor += directionVec;
        }

        return true;
    }

    private bool CheckSetWordInputsValid(LetterLocation fromLocation, Vector2Int directionVec, int wordLength)
    {
        var toLocation = fromLocation + directionVec * (wordLength - 1);
        return (m_LetterMap.ContainsKey(fromLocation) && m_LetterMap.ContainsKey(toLocation));     
    }
    
    private void Awake()
    {
        SetElementScalingAndSpacing();
        InitLetterMap();
    }

    private (float, float) GetFrustumHeightAndWidth()
    {
        float frustumHeight = 2.0f * Camera.main.orthographicSize;
        float frustumWidth = frustumHeight * Camera.main.aspect;

        return (frustumHeight, frustumWidth);
    }

    private void SetElementScalingAndSpacing()
    {
        (float frustumHeight, float frustumWidth) = GetFrustumHeightAndWidth();

        float elementHeight = 1;
        float elementWidth = 1;

        float rawWholeSize = edgeLength * elementHeight + (edgeLength - 1) * spacing;
        float columnWholeSize = edgeLength * elementWidth + (edgeLength - 1) * spacing;

        float scaleOfElementsIfRow = frustumHeight / rawWholeSize;
        float scaleOfElementsIfColumn = frustumWidth / columnWholeSize;

        // scale less to void floating point errors
        const float scaleErrorMargin = 0.05f;

        m_ElementScaling = Math.Min(scaleOfElementsIfColumn, scaleOfElementsIfRow) - scaleErrorMargin;
        m_ElementSpacing = m_ElementScaling + spacing;
    }

    private void InitLetterMap()
    {
        m_LetterMap = new Dictionary<LetterLocation, Transform>(edgeLength * edgeLength);
        CreateRandomLetters();
        AdjustRandomLetterPositions();
    }

    private void CreateRandomLetters()
    {
        foreach (int col in Enumerable.Range(0, edgeLength))
        {
            foreach (int row in Enumerable.Range(0, edgeLength))
            {
                var randomLetter = Letter<char>.GenerateRandomLetter();
                var letterCube = letterGenerator.Generate(randomLetter, Vector3.zero, Quaternion.identity);
                LetterLocation location = new LetterLocation (col, row);
                m_LetterMap.Add(location, letterCube);
            }
        }
    }

    private void AdjustRandomLetterPositions()
    {
        foreach (KeyValuePair<LetterLocation, Transform> positionLetterCubeKeyVal in m_LetterMap)
        {
            var location = positionLetterCubeKeyVal.Key;
            var letterCube = positionLetterCubeKeyVal.Value;

            SetLetterModelPosition(letterCube, location);
        }
    }

    private void SetLetterModelPosition(Transform letterCube, LetterLocation location)
    {
        Vector3 origin = new Vector3(
            (float)m_ElementSpacing * edgeLength / 2,
            (float)m_ElementSpacing * edgeLength / 2,
            0f);

        float xPos = (float)m_ElementSpacing * location.column - origin.x;
        float yPos = (float)m_ElementSpacing * location.row - origin.y;

        letterCube.position = new Vector3(xPos, yPos, 0f);
        letterCube.localScale *= (float)m_ElementScaling;
    }
}
