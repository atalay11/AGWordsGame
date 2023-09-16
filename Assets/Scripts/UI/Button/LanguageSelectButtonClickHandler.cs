using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelectButtonClickHandler : MonoBehaviour
{
    [SerializeField] private LocalizationManager.Language language;
    public void OnButtonClick()
    {
        LocalizationManager.Instance.SetLanguageManually(language);
        MainMenu.Instance.OnLanguageSelected();
    }
}

