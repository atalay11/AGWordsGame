using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject letterMap;
    [SerializeField] private GameObject wordDatabase;

    EventHandler OnWordSelectedEvent;

    public void NewBoard(int edgeLength)
    {
        letterMap.GetComponent<LetterMap>().ResetWithNewEdgeLength(edgeLength);
    }

    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            var fromLoc = new LetterLocation(0, 3);
            
            string randomWord;
            do 
            {
               randomWord = wordDatabase.GetComponent<WordDatabase>().GetRandomWord();
            }
            while (randomWord.Length > 10);
            
            letterMap.GetComponent<LetterMap>().SetWord(randomWord, fromLoc, Direction.Right);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            NewBoard(20);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            NewBoard(15);
        }
    }
}
