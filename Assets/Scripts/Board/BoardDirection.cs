using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Unknown,
    Up,
    Down,
    Right,
    Left,
    UpperRight,
    UpperLeft,
    DownRight,
    DownLeft
}

public static class BoardDirection
{
    public static readonly Direction[] directionList = 
    {
        Direction.Up, Direction.Down, Direction.Right, Direction.Left, 
        Direction.UpperRight, Direction.UpperLeft, Direction.DownRight, Direction.DownLeft
    };
    public static readonly Dictionary<Direction, Vector2Int> directionMappings = new Dictionary<Direction, Vector2Int>
    {
        { Direction.Up, new Vector2Int(0, 1) },
        { Direction.Down, new Vector2Int(0, -1) },
        { Direction.Right, new Vector2Int(1, 0) },
        { Direction.Left, new Vector2Int(-1, 0) },
        { Direction.UpperRight, new Vector2Int(1, 1) },
        { Direction.UpperLeft, new Vector2Int(-1, 1) },
        { Direction.DownRight, new Vector2Int(1, -1) },
        { Direction.DownLeft, new Vector2Int(-1, -1) }
    };

    public static Vector2Int GetVector(Direction direction) => directionMappings[direction];
}