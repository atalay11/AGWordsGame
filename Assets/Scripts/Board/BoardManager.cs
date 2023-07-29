using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject letterMap;

    private void Awake()
    {
        CoreWordnet.Initilize();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var fromLoc = new LetterLocation(0, 2);
            letterMap.GetComponent<LetterMap>().SetWord("TEST_A", fromLoc, Direction.UpperRight);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            var fromLoc = new LetterLocation(0, 3);
            letterMap.GetComponent<LetterMap>().SetWord("0000000000", fromLoc, Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            var fromLoc = new LetterLocation(7, 7);
            letterMap.GetComponent<LetterMap>().SetWord("TEST_C", fromLoc, Direction.DownLeft);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            var fromLoc = new LetterLocation(6, 2);
            letterMap.GetComponent<LetterMap>().SetWord("TEST_D", fromLoc, Direction.Left);
        }
    }
}
