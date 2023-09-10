using UnityEngine;

public class WordHintPair : ScriptableObject
{
    [field: SerializeField]
    public string Word { get; private set; }

    [field: SerializeField]
    public string Hint { get; private set; }

}