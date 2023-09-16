using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using System;
using System.Globalization;

public class WordDatabase : GenericSingleton<WordDatabase>
{
    // protected override void AwakeImpl()
    // {
    //     Initialize();
    // }

    private void Start()
    {
        Initialize();
    }
    
    public void Initialize()
    {
        m_WordSet = new HashSet<string>();
        PrepareWordsForLanguage(LocalizationManager.Instance.CurrentLanguage);
        LocalizationManager.Instance.OnLanguageChangeEvent += LocalizationManager_OnLanguageChangeEvent;
    }

    private void LocalizationManager_OnLanguageChangeEvent(object sender, LocalizationManager.OnLanguageChangeEventArgs e)
    {
        PrepareWordsForLanguage(e.newLanguage);
    }

    public string GetRandomWord()
    {
        // Randomly select word type
        return m_WordSet.ElementAt(m_Random.Next(m_WordSet.Count));
    }

    private void PrepareWordsForLanguage(LocalizationManager.Language language)
    {
        m_WordSet.Clear();

        if (m_LanguageToWordsFileMap.TryGetValue(language, out string filePath))
        {    
            TextAsset csvFile = Resources.Load<TextAsset>(filePath);
            string[] words = csvFile.text.Split('\n').ToArray(); ; // Split the file into lines

            foreach (string word in words)
            {
                m_WordSet.Add(word.Trim().ToUpper(m_CultureInfoMap[language]));
            }
        }
        else
        {
            throw new NotImplementedException($"Language {language} is not available.");
        }
    }

    private HashSet<string> m_WordSet;

    private readonly System.Random m_Random = new System.Random();
    private readonly Dictionary<LocalizationManager.Language, string> m_LanguageToWordsFileMap = new Dictionary<LocalizationManager.Language, string>
    {
        { LocalizationManager.Language.English, "CoreWordnet/core-wordnet" },
        { LocalizationManager.Language.Turkish, "TurkishWords/turkish_words" }
    };  

    private readonly Dictionary<LocalizationManager.Language, CultureInfo> m_CultureInfoMap = new Dictionary<LocalizationManager.Language, CultureInfo>
    {
        { LocalizationManager.Language.English, new CultureInfo("en-US") },
        { LocalizationManager.Language.Turkish, new CultureInfo("tr-TR") }
    };
}
