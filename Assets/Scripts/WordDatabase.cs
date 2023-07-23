using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class CoreWordnet
{
    static HashSet<string> adjs;
    static HashSet<string> nouns;
    static HashSet<string> verbs;

    public static void Initilize()
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

    private static void ReadCoreWordnet(TextAsset csvFile)
    {
        // First row is consist of column names 
        string[] lines = csvFile.text.Split('\n').Skip(1).ToArray(); ; // Split the file into lines

        foreach (string line in lines)
        {
            string[] values = line.Split(','); // Split the line into values

            var form = values[0];
            var word = values[1];

            if (form == "a")
                adjs.Add(word);
            else if (form == "n")
                nouns.Add(word);
            else if (form == "v")
                verbs.Add(word);

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

}
