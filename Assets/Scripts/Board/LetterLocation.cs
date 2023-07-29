using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct LetterLocation
{
    public int column;
    public int row;

    public LetterLocation(int columnIndex, int rowIndex)
    {
        column = columnIndex;
        row = rowIndex;
    }

    public static LetterLocation operator +(LetterLocation l, Vector2Int v)
    {
        return new LetterLocation(l.column + v.x , l.row + v.y);
    }

    public static LetterLocation operator -(LetterLocation l, Vector2Int v)
    {
        return new LetterLocation(l.column - v.x, l.row - v.y);
    }

    public static Vector2Int operator -(LetterLocation l1, LetterLocation l2)
    {
        return new Vector2Int(l1.column - l2.column, l1.row - l1.row);
    }
}