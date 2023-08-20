using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
    // Start is called before the first frame update
    
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update() 
    {
        // Temporary Language changer for each touch down
        TempLanguageChanger();
    }

    private void TempLanguageChanger()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                if (LocalizationManager.Instance.CurrentLanguage == LocalizationManager.Language.English)
                {
                    LocalizationManager.Instance.SetLanguageManually(LocalizationManager.Language.Turkish);
                }
                else
                {
                    LocalizationManager.Instance.SetLanguageManually(LocalizationManager.Language.English);
                }
            }
        }
    }
}
