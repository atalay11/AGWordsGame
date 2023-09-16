using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterUtils : GenericSingleton<LetterUtils>
{
    public char GenerateRandomLetter()
    {
        if (languageUppercaseMappings.TryGetValue(LocalizationManager.Instance.CurrentLanguage, out string uppercaseLetters))
        {
            int randomIndex = UnityEngine.Random.Range(0, uppercaseLetters.Length);
            return uppercaseLetters[randomIndex];
        }

        // Default to English letters if language not found
        return languageUppercaseMappings[LocalizationManager.Language.English][UnityEngine.Random.Range(0, 26)];
    }

    private Dictionary<LocalizationManager.Language, string> languageUppercaseMappings = new Dictionary<LocalizationManager.Language, string>
    {
        { LocalizationManager.Language.English, "ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
        { LocalizationManager.Language.Turkish, "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ" }
    };

}