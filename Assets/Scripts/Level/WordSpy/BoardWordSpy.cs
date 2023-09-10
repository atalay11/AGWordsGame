using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class BoardWordSpy : MonoBehaviour
{
    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        (lastWindowHeight, lastWindowWidth) = GetFrustumHeightAndWidth();
    }

    private void Update()
    {
        // need update
        if (lastWindowHeight == 0)
        {
            (float curWindowHeight, float curWindowWidth) = GetFrustumHeightAndWidth();

            if (curWindowHeight != 0)
            {
                lastWindowHeight = curWindowHeight;
                lastWindowWidth = curWindowWidth;

                SetElementScalingAndSpacing();
                AdjustRandomLetterPositions();
            }
        }

    }

    public void ResetWithNewEdgeLength(int newEdgeLength)
    {
        m_EdgeLength = newEdgeLength;
        m_gridLayoutGroup.constraintCount = newEdgeLength;
        ResetLetters();
    }

    // Return false if no such toLocation
    public bool SetLetter(char letter, LetterLocation toLocation)
    {
        if (m_LetterMap.TryGetValue(toLocation, out Transform letterCubeTransform))
        {
            letterCubeTransform.GetComponent<LetterCube>().SetLetter(letter);
            return true;
        }
        return false;
    }

    public char GetLetter(LetterLocation fromLocation)
    {
        var transform = m_LetterMap[fromLocation];
        return transform.GetComponent<LetterCube>().GetLetter();
    }

    public bool DoesWordOverlap(string word, LetterCube letterCube, Direction toDirection)
    {
        LetterLocation? fromLocation = m_LetterMap.FirstOrDefault(x => x.Value.Equals(letterCube.transform)).Key;
        if (!fromLocation.HasValue)
        {
            Debug.LogError($"letterCube can not be located!");
            return false;
        }

        var directionVec = BoardDirection.GetVector(toDirection);
        if (!CheckSetWordInputsValid(fromLocation.Value, directionVec, word.Length))
        {
            Debug.LogError($"SetWord inputs are invalid. fromLoc: `({fromLocation.Value.column}, {fromLocation.Value.row})`, word: `{word}`, directionVec: `{directionVec}`");
            return false;
        }

        var locationCursor = fromLocation.Value;
        foreach (char ch in word)
        {
            var transform = m_LetterMap[locationCursor];
            char letter = transform.GetComponent<LetterCube>().GetLetter();
            if (letter != ch)
                return false;
            locationCursor += directionVec;
        }

        return true;
    }

    public bool SetWord(string word, LetterLocation fromLocation, Direction toDirection)
    {
        if (string.IsNullOrEmpty(word) || toDirection == Direction.Unknown)
        {
            Debug.LogWarning($"word is null or empty.");
            return false;
        }

        var directionVec = BoardDirection.GetVector(toDirection);
        if (!CheckSetWordInputsValid(fromLocation, directionVec, word.Length))
        {
            return false;
        }

        var locationCursor = fromLocation;
        bool placeable = true;
        foreach (char ch in word)
        {
            var letter = GetLetter(locationCursor);
            placeable &= letter == '?' || ch == letter;
            locationCursor += directionVec;
        }

        if (!placeable)
        {
            return false;
        }

        locationCursor = fromLocation;
        foreach (char ch in word)
        {
            bool success = SetLetter(ch, locationCursor);
            locationCursor += directionVec;
        }

        return true;
    }

    public LetterLocation GenerateRandomLetterLocation()
    {
        return new LetterLocation(UnityEngine.Random.Range(0, m_EdgeLength), UnityEngine.Random.Range(0, m_EdgeLength));
    }

    public void CleanBoard()
    {
        foreach (int col in Enumerable.Range(0, m_EdgeLength))
        {
            foreach (int row in Enumerable.Range(0, m_EdgeLength))
            {
                SetLetter('?', new LetterLocation(col, row));
            }
        }
    }

    public void FillEmptyLetters()
    {
        foreach (int col in Enumerable.Range(0, m_EdgeLength))
        {
            foreach (int row in Enumerable.Range(0, m_EdgeLength))
            {
                var locationCursor = new LetterLocation(col, row);
                if (GetLetter(locationCursor) == '?')
                {
                    SetLetter(LetterUtils.Instance.GenerateRandomLetter(), locationCursor);
                }
            }
        }
    }

    private bool CheckSetWordInputsValid(LetterLocation fromLocation, Vector2Int directionVec, int wordLength)
    {
        var toLocation = fromLocation + directionVec * (wordLength - 1);
        return m_LetterMap.ContainsKey(fromLocation) && m_LetterMap.ContainsKey(toLocation);
        // return toLocation.column >= 0 && toLocation.column < edgeLength
        //     && toLocation.row >= 0 && toLocation.row < edgeLength;
    }

    private void ResetLetters()
    {
        ClearLetterMap();
        SetElementScalingAndSpacing();
        InitLetterMap();
    }

    private void ClearLetterMap()
    {
        if (m_LetterMap == null)
            return;

        foreach (var transform in m_LetterMap.Values)
        {
            Destroy(transform.gameObject);
        }

        m_LetterMap.Clear();
    }

    private (float, float) GetFrustumHeightAndWidth()
    {
        return (m_rectTransform.rect.height, m_rectTransform.rect.width);
    }

    private void SetElementScalingAndSpacing()
    {
        if (lastWindowHeight == 0)
            return;

        (float frustumHeight, float frustumWidth) = GetFrustumHeightAndWidth();
        var boardPadding = m_gridLayoutGroup.padding;
        var spacing = m_gridLayoutGroup.spacing.x;
        var spacingSpace = (m_EdgeLength - 1) * spacing;
        frustumHeight -= (boardPadding.top + boardPadding.bottom + spacingSpace);
        frustumWidth -= (boardPadding.left + boardPadding.right + spacingSpace);

        float elementHeight = 1;
        float elementWidth = 1;

        float elementsWholeHeightSize = m_EdgeLength * elementHeight;
        float elementsWholeWidthSize = m_EdgeLength * elementWidth;

        float scaleOfElementsIfHeight = frustumHeight / elementsWholeHeightSize;
        float scaleOfElementsIfWidth = frustumWidth / elementsWholeWidthSize;

        // scale less to void floating point errors
        const float scaleErrorMargin = 0.05f;

        m_ElementScaling = Math.Min(scaleOfElementsIfHeight, scaleOfElementsIfWidth) - scaleErrorMargin;
    }

    private void InitLetterMap()
    {
        m_LetterMap = new Dictionary<LetterLocation, Transform>(m_EdgeLength * m_EdgeLength);
        InitializeBoard();
        AdjustRandomLetterPositions();
    }

    private void InitializeBoard()
    {
        foreach (int col in Enumerable.Range(0, m_EdgeLength))
        {
            foreach (int row in Enumerable.Range(0, m_EdgeLength))
            {
                var randomLetter = LetterUtils.Instance.GenerateRandomLetter();
                var letterCube = letterGenerator.Generate(randomLetter, board);
                LetterLocation location = new LetterLocation(col, row);
                m_LetterMap.Add(location, letterCube);
            }
        }
    }

    private void AdjustRandomLetterPositions()
    {
        if (lastWindowHeight == 0)
            return;

        foreach (KeyValuePair<LetterLocation, Transform> locationLetterCubeKeyVal in m_LetterMap)
        {
            var location = locationLetterCubeKeyVal.Key;
            var letterCube = locationLetterCubeKeyVal.Value;

            SetLetterModelPosition(letterCube, location);
        }
    }

    private void SetLetterModelPosition(Transform letterCube, LetterLocation location)
    {
        m_gridLayoutGroup.cellSize = new Vector2(m_ElementScaling, m_ElementScaling);
        letterCube.localScale *= (float)m_ElementScaling;
    }

    // Variables

    [SerializeField] private LetterGenerator letterGenerator;
    [SerializeField] private Transform board;

    private RectTransform m_rectTransform;
    private GridLayoutGroup m_gridLayoutGroup;
    private Dictionary<LetterLocation, Transform> m_LetterMap;    // 0, 0 is bottom-left
    private float m_ElementScaling;
    private int m_EdgeLength; // board size will be -> edgeLength - edgeLength (width - height)
    private float lastWindowHeight = 0.0f;
    private float lastWindowWidth = 0.0f;
}
