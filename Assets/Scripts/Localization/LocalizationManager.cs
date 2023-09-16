using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    // ?? Can we use SystemLanguage enum instead 
    public enum Language : int
    {
        English = 0,
        Turkish
    }

    public EventHandler<OnLanguageChangeEventArgs> OnLanguageChangeEvent;
    public class OnLanguageChangeEventArgs : EventArgs
    {
        public Language oldLanguage;
        public Language newLanguage;
    }

    public void SetLanguageManually(Language language)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)language];
        OnLanguageChangeEvent?.Invoke(this, new OnLanguageChangeEventArgs{oldLanguage = CurrentLanguage, newLanguage = language});
        CurrentLanguage = language;
        SavePreferedLanguage(language);
        Debug.LogFormat("Language is changed to: `{0}`", LocalizationSettings.SelectedLocale.name);
    }

    public void SavePreferedLanguage(Language language)
    {
        var prefs = PersistanceManager.Instance.GetUserPreferences();

        if (language == Language.English)
            prefs.language = SystemLanguage.English;
        else if (language == Language.Turkish)
            prefs.language = SystemLanguage.Turkish;

        PersistanceManager.Instance.SetUserPreferences(prefs);
    }

    public void SetPreferedLanguage()
    {
        var prefs = PersistanceManager.Instance.GetUserPreferences();

        Language language;
        if (prefs.language == SystemLanguage.English)
           language = Language.English;
        else if (prefs.language == SystemLanguage.Turkish)
           language = Language.Turkish;
        else
        {
           language = Language.English;
           Debug.LogError($"No such language: `{prefs.language}` is found. Setting defaul language ({SystemLanguage.English})");
        }
            
        SetLanguageManually(language);
    }

    IEnumerator Start()
    {
        // Wait for the localization system to initialize, loading Locales, preloading etc.
        yield return LocalizationSettings.InitializationOperation;
        SetPreferedLanguage();
    }


    private Language currentLanguage = Language.English;
}