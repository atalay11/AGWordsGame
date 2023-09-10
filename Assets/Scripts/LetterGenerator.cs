using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGenerator : MonoBehaviour
{
    [SerializeField] private Transform letterPrefab;

    public Transform Generate(char letter)
    {
        var newLetterCube = Instantiate(letterPrefab);
        var letterCube = newLetterCube.GetComponent<LetterCube>();
        letterCube.SetLetter(letter);
        return newLetterCube;
    }

    public Transform Generate(char letter, Transform parent)
    {
        var newLetterCube = Instantiate(letterPrefab, parent);
        var letterCube = newLetterCube.GetComponent<LetterCube>();
        letterCube.SetLetter(letter);
        return newLetterCube;
    }

}