using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveTemplateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveTmp;

    public void SetText(string text)
    {
        objectiveTmp.text = text;
    }
}
