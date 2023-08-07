using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject letterSelectionChecker;

    public void NewBoard(int edgeLength)
    {
        board.GetComponent<Board>().ResetWithNewEdgeLength(edgeLength);
    }

    private void Start()
    {
        letterSelectionChecker.GetComponent<LetterSelectionChecker>().OnWordSelected += LetterSelectionChecker_OnWordSelected;
    }

    private void LetterSelectionChecker_OnWordSelected(object sender, LetterSelectionChecker.OnWordSelectedEventArgs e)
    {
        if (e.direction == Direction.Unknown)
            return;
        // Extra check, this might be unnecessary
        bool overlaps = board.GetComponent<Board>().DoesWordOverlap(e.word, e.firstLetterCube, e.direction); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            string randomWord;
            do 
            {
               randomWord = WordDatabase.Instance.GetRandomWord();
            }
            while (randomWord.Length > 10);
            
            board.GetComponent<Board>().SetWord(randomWord, new LetterLocation(0, 3), Direction.Right);
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
