using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WordTile : ScriptableObject
{
    private WordConnectGridManager _gridManager;

    private bool _isFound;

    public WordHintPair WordHint { get; private set; }

    private List<LetterTile> _letterTiles;

    public List<LetterTile> LetterTiles => _letterTiles;

    private Dictionary<LetterTile, WordTile> _overlappingElements;

    public static WordTile CreateWordTile(WordHintPair wordHintPair)
    {
        WordTile wordTile = CreateInstance<WordTile>();
        wordTile.WordHint = wordHintPair;
        wordTile._isFound = false;
        wordTile._letterTiles = new List<LetterTile>();
        wordTile._overlappingElements = new Dictionary<LetterTile, WordTile>();

        return wordTile;
    }

    public void SetupTile(WordConnectGridManager gridManager) => _gridManager = gridManager;

    public void GameStateUpdated(WordConnectState state)
    {
        if (state.CurrentWordInput.Count > 0) return;
        CheckIfWordFound(state);
    }

    private void CheckIfWordFound(WordConnectState state)
    {
        if (state.CorrectlyAddedWords.Contains(WordHint.Word, StringComparer.OrdinalIgnoreCase) && !_isFound)
        {
            _isFound = true;
            SetWordFound();
        }
    }

    private void SetWordFound()
    {
        foreach (LetterTile letterTile in _letterTiles)
        {
            letterTile.IsFound = true;
            if (letterTile.gameObject.activeInHierarchy)
                letterTile.AnimateColors(0.1f);
            else
                letterTile.UpdateColors();
        }
        bool allLetterTilesActive = _letterTiles.All(tile => tile.gameObject.activeInHierarchy);

        // if (allLetterTilesActive)
    }
    public void AddLetter(LetterTile letterTile) => _letterTiles.Add(letterTile);
    public void ConnectOverlappingWordTile(LetterTile letterTile, WordTile wordTile) => _overlappingElements.Add(letterTile, wordTile);
    public bool ContainsLetterTile(LetterTile letterTile) => _letterTiles.Contains(letterTile);

}
