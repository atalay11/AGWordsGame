using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct WordVector
{
    [field: SerializeField]
    public Vector2Int Origin { get; private set; }

    [field: SerializeField]
    public bool IsDown { get; private set; }

    [field: SerializeField]
    public List<Vector2Int> OccupiedPositions { get; private set; }

    [field: SerializeField]
    public List<Vector2Int> AffectedPositions { get; private set; }

    [field: SerializeField]
    public WordHintPair WordHintPair { get; private set; }

    public WordVector(WordHintPair wordHintPair, Vector2Int origin, bool isDown)
    {
        WordHintPair = wordHintPair;
        Origin = origin;
        IsDown = isDown;

        OccupiedPositions = new List<Vector2Int>();
        AffectedPositions = new List<Vector2Int>();

        CalculatePositions();
    }

    public WordVector DeepCopy() => new WordVector(WordHintPair, Origin, IsDown);

    public void CalculatePositions()
    {
        CalculateOccupiedPositions();
        CalculateAffectedPositions();
    }

    private void CalculateOccupiedPositions()
    {
        if (OccupiedPositions == null) OccupiedPositions = new List<Vector2Int>();
        if (OccupiedPositions.Count > 0) OccupiedPositions.Clear();

        for (int i = 0; i < WordHintPair.Word.Length; i++)
        {
            Vector2Int newPosition = Origin + new Vector2Int(IsDown ? 0 : i, IsDown ? -i : 0);
            OccupiedPositions.Add(newPosition);
        }

    }

    private void CalculateAffectedPositions()
    {
        AffectedPositions.Clear();

        for (int i = 0; i < OccupiedPositions.Count; i++)
        {
            Vector2Int origin = OccupiedPositions[i];

            if (IsDown)
            {
                AffectedPositions.Add(origin + Vector2Int.left);
                AffectedPositions.Add(origin + Vector2Int.right);

                if (i == 0)
                    AffectedPositions.Add(origin + Vector2Int.up);
                else if (i == OccupiedPositions.Count - 1)
                    AffectedPositions.Add(origin + Vector2Int.down);
            }
            else
            {
                AffectedPositions.Add(origin + Vector2Int.up);
                AffectedPositions.Add(origin + Vector2Int.down);

                if (i == 0)
                    AffectedPositions.Add(origin + Vector2Int.left);
                else if (i == OccupiedPositions.Count - 1)
                    AffectedPositions.Add(origin + Vector2Int.right);
            }
        }
    }

    public bool CheckForOverlap(WordVector givenWordVector)
    {
        foreach (Vector2Int item in AffectedPositions)
            if (givenWordVector.OccupiedPositions.Contains(item))
                return true;

        foreach (Vector2Int item in OccupiedPositions)
            if (givenWordVector.OccupiedPositions.Contains(item))
                return true;

        return false;
    }
}