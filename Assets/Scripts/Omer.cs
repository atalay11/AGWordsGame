using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Omer : MonoBehaviour
{
    [SerializeField] private LetterGenerator letterGenerator;

    List<Transform> letterCubes;
    float timePassedWordCreation = wordCreationInterval + 1f; // words are generated at game start


    const float wordCreationInterval = 10f;
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
        timePassedWordCreation = 0;
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


        const float margin = 4f;
        float lastXpos = -margin;
        float lastYpos = margin;
        int batchNumber = 0;
        const int batchSize = 8;
        foreach (var letter in GenerateRandomString())
        {
            var letterCube = letterGenerator.Generate(letter);
            letterCube.position = new Vector3(lastXpos, lastYpos, 0f);
            letterCubes.Add(letterCube);
            lastXpos += space;

            batchNumber++;
            if (batchNumber == batchSize)
            {
                lastYpos -= space;
                lastXpos = -margin;
                batchNumber = 0;
            }
        }
    }

    private string GenerateRandomString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 64)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

}
