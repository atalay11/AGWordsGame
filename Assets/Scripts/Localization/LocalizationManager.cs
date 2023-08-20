using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;


public class LocalizationManager : GenericSingleton<LocalizationManager>
{
    // Must be in the same order with `LocalizationSettings.AvailableLocales.Locales`

    public Language CurrentLanguage 
    {
        get { return currentLanguage; }
        private set { currentLanguage = value; }
    }

    public enum Language : int
    {
        English = 0,
        Turkish
    }

    public void SetLanguageManually(Language language)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)language];
        CurrentLanguage = language;
        Debug.LogFormat("Language is changed to: `{0}`", LocalizationSettings.SelectedLocale.name);
    }

    IEnumerator Start()
    {
        // Wait for the localization system to initialize, loading Locales, preloading etc.
        yield return LocalizationSettings.InitializationOperation;
    }


    private Language currentLanguage = Language.English;
}