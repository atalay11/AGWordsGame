using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterUtils : GenericSingleton<LetterUtils>
{
    public char GenerateRandomLetter()
    {
        if (languageUppercaseMappings.TryGetValue(Application.systemLanguage, out string uppercaseLetters))
        {
            int randomIndex = UnityEngine.Random.Range(0, uppercaseLetters.Length);
            return uppercaseLetters[randomIndex];
        }

        // Default to English letters if language not found
        return languageUppercaseMappings[SystemLanguage.English][UnityEngine.Random.Range(0, 26)];
    }

    private Dictionary<SystemLanguage, string> languageUppercaseMappings = new Dictionary<SystemLanguage, string>
    {
        { SystemLanguage.English, "ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
        { SystemLanguage.Turkish, "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ" },
        { SystemLanguage.Spanish, "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ" },
        { SystemLanguage.Russian, "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ" }
    };

}