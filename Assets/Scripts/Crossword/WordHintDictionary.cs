using UnityEngine;

public class WordHintDictionary : ScriptableObject
{
    [field: SerializeField]
    public string DictionaryName { get; private set; }

    [field: SerializeField]
    public WordHintPair[] WordHintPairs { get; private set; }
}
