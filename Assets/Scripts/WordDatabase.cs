using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class WordDatabase : MonoBehaviour
{
    public enum WordType : int
    {
        Adj,
        Noun,
        Verb,
        WordTypeNB
    }

    public static WordDatabase Instance { get; private set; }

    private void Awake()
    {
        // If an instance already exists, destroy the newly created one.
        if (Instance != null && Instance != this)
        {
            Debug.LogError("WordDatabase is singleton but tried to be set more than once.");
            return;
        }

        // Set the instance to this object if it's null.
        Instance = this;
        Initialize();
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        adjs = new HashSet<string>();
        nouns = new HashSet<string>();
        verbs = new HashSet<string>();

        TextAsset coreWordnet = Resources.Load<TextAsset>("CoreWordnet/core-wordnet");
        ReadCoreWordnet(coreWordnet);

        Debug.Log($"adj {adjs.Count}");
        Debug.Log($"nouns {nouns.Count}");
        Debug.Log($"verbs {verbs.Count}");
    }

    public string GetRandomWord()
    {
        // Randomly select word type
        WordType wordType = (WordType)m_Random.Next((int)WordType.WordTypeNB);

        if (wordType == WordType.Adj)
        {
            return adjs.ElementAt(m_Random.Next(adjs.Count));
        }
        else if (wordType == WordType.Noun)
        {
            return nouns.ElementAt(m_Random.Next(nouns.Count));
        }
        else if (wordType == WordType.Verb)
        {
            return verbs.ElementAt(m_Random.Next(verbs.Count));
        }
        else
        {
            throw new System.NotImplementedException($"wordType: `{wordType}` is not implemented yet.");
        }
    }

    public static HashSet<string> GetAdjs()
    {
        return adjs;
    }

    public static HashSet<string> GetNouns()
    {
        return nouns;
    }

    public static HashSet<string> GetVerbs()
    {
        return verbs;
    }


    private static void ReadCoreWordnet(TextAsset csvFile)
    {
        // First row is consist of column names 
        string[] lines = csvFile.text.Split('\n').Skip(1).ToArray(); ; // Split the file into lines

        foreach (string line in lines)
        {
            string[] values = line.Split(','); // Split the line into values

            var form = values[0];
            var word = values[1].Trim().ToUpper();

            if (form == "a")
                adjs.Add(word);
            else if (form == "n")
                nouns.Add(word);
            else if (form == "v")
                verbs.Add(word);

        }
    }

    private static HashSet<string> adjs;
    private static HashSet<string> nouns;
    private static HashSet<string> verbs;

    private readonly System.Random m_Random = new System.Random();
}
