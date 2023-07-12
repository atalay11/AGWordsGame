using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterCube : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] sides;


    public void SetLetter(char letter)
    {
        foreach (var side in sides)
        {
            side.text = letter.ToString();
        }
    }

}
