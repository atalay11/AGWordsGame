using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Omer : MonoBehaviour
{
    [SerializeField] private LetterGenerator letterGenerator;

    List<Transform> letterCubes;
    float timePassedWordCreation = 0f;


    const float wordCreationInterval = 1f;
    System.Random random; // lol

    private void Awake()
    {
        letterCubes = new List<Transform>();
        random = new System.Random();
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
            timePassedWordCreation = 0;
        }
    }

    private void GenerateRandomWordCubes()
    {
        const float space = 1.1f;

        // clear the old cubes
        foreach (var letterCube in letterCubes)
        {
            Destroy(letterCube.gameObject);
        }
        letterCubes.Clear();



        float lastXpos = 0f;
        foreach (var letter in GenerateRandomString())
        {
            var letterCube = letterGenerator.Generate(letter);
            letterCube.position = new Vector3(lastXpos, 0f, 0f);
            letterCubes.Add(letterCube);
            lastXpos += space;
        }
    }

    private string GenerateRandomString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, random.Next(5, 10))
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

}
