using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Omer : MonoBehaviour
{
    [SerializeField] private LetterGenerator letterGenerator;

    List<Transform> letterCubes;
    float timePassedWordCreation = wordCreationInterval + 1f; // words are generated at game start

    const float wordCreationInterval = 2f;
    System.Random random; // lol

    private void Awake()
    {
        letterCubes = new List<Transform>();
        random = new System.Random();

        CoreWordnet.Initilize();
    }

    private void Start()
    {
    }

    private void Update()
    {
        timePassedWordCreation += Time.deltaTime;

        if (timePassedWordCreation > wordCreationInterval)
        {
            GenerateRandomWordCubes();
            SetLetterLayout();
            timePassedWordCreation = 0;
        }
        timePassedWordCreation = 0;
    }

    private void GenerateRandomWordCubes()
    {
        // clear the old cubes
        foreach (var letterCube in letterCubes)
        {
            Destroy(letterCube.gameObject);
        }
        letterCubes.Clear();

        foreach (var letter in GenerateRandomString())
        {
            var letterCube = letterGenerator.Generate(letter);
            letterCube.position = Vector3.zero;
            letterCube.rotation = Quaternion.identity;
            letterCubes.Add(letterCube);
        }
    }

    private string GenerateRandomString()
    {
        int numberOfLetters = random.Next(10, 20);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, (int)Mathf.Pow(numberOfLetters, 2))
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private void SetLetterLayout()
    {
        var mainCamera = Camera.main;

        float frustumHeight = 2.0f * mainCamera.orthographicSize;
        float frustumWidth = frustumHeight * mainCamera.aspect;

        // always want square grid
        var maxColumnSize = (int)Mathf.Sqrt(letterCubes.Count);

        float spacing = 0.05f;
        GridLayout3D.SetLayout(letterCubes, frustumHeight, frustumWidth, maxColumnSize, spacing);
    }

}
