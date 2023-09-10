using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(WordConnectManager))]
public class WordConnectGridManager : MonoBehaviour
{
    private WordConnectManager _wordConnectManager;

    [SerializeField]
    private GameObject _letterTile;

    [SerializeField]
    private GameObject _blockedTile;

    [SerializeField]
    private WordConnectConfigurationData _gameData;

    [SerializeField]
    private GridLayoutGroup _gridLayout;

    public GridLayoutGroup GridLayout => _gridLayout;


    private WordConnectLayout _gameLayout;

    private List<WordTile> _wordTiles;

    public List<WordTile> WordTiles => _wordTiles;

    private List<LetterTile> _letterTiles;


    public void GenerateNewVectorLayout() => _gameLayout.SetWordVectors(CrosswordAlgorithm.GetVectorCrosswordLayout(_gameData.DictionaryAsset));

    public void ChangeGameConfiguration(WordConnectConfigurationData givenConfiguration) => _gameData = givenConfiguration;

    public void LoadLayoutFromGameData() => _gameLayout.UpdateLayout(_gameData.ReturnLayout());

    public void SaveLayoutToGameData()
    {
        _gameData.SetWordVectors(_gameLayout.WordVectors);
        _gameData.SetCrosswordLetters(_gameLayout.GridLetters);
    }

    private void Awake() => _gameLayout = new WordConnectLayout();

    private void OnEnable()
    {
        _wordConnectManager = WordConnectManager.Instance;
        _wordConnectManager.BuildGame += StartGame;
    }

    private void OnDisable() => _wordConnectManager.BuildGame -= StartGame;

    private void StartGame()
    {
        ChangeGameConfiguration(_wordConnectManager.Configuration);
        LoadLayoutFromGameData();
        BuildActiveLayouts();
    }

    public void DisposeWordTiles(List<WordTile> wordTiles, bool nullReset)
    {
        if (wordTiles == null)
            return;

        for (int i = 0; i < wordTiles.Count; i++)
        {
            _wordConnectManager.StateUpdated -= wordTiles[i].GameStateUpdated;
            Destroy(wordTiles[i]);
        }

        if (nullReset)
            wordTiles = null;
        else
            wordTiles.Clear();
    }

    public void DisposeLetterTiles(List<LetterTile> letterTiles, bool nullReset)
    {
        if (letterTiles == null)
            return;

        for (int i = 0; i < _letterTiles.Count; i++)
        {
            Destroy(letterTiles[i]);
        }

        if (nullReset)
            letterTiles = null;
        else
            letterTiles.Clear();
    }

    public void BuildActiveLayouts()
    {
        _gridLayout.gameObject.SetActive(Application.isPlaying); // test

        DisposeWordTiles(_wordTiles, true);
        DisposeLetterTiles(_letterTiles, true);

        SetupLayoutSettings(_gridLayout);

        BuildLayout(_gridLayout, out _wordTiles, out _letterTiles);
    }

    public void SetupLayoutSettings(GridLayoutGroup layoutGroup)
    {
        layoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        layoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
        layoutGroup.childAlignment = TextAnchor.LowerLeft;
    }

    public void BuildLayout(GridLayoutGroup parent, out List<WordTile> wordTiles, out List<LetterTile> letterTiles)
    {
        wordTiles = new List<WordTile>();
        letterTiles = new List<LetterTile>();

        for (int i = parent.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(parent.transform.GetChild(i).gameObject);

        int xUpperBound = _gameLayout.WordVectors.Max(x => x.OccupiedPositions.Last().x);
        int xLowerBound = _gameLayout.WordVectors.Min(x => x.OccupiedPositions.First().x);
        int yUpperBound = _gameLayout.WordVectors.Max(y => y.OccupiedPositions.First().y);
        int yLowerBound = _gameLayout.WordVectors.Min(y => y.OccupiedPositions.Last().y);

        Vector2Int vectorOffset = Vector2Int.zero;

        vectorOffset.x -= (xLowerBound < 0) ? xLowerBound : 0;
        vectorOffset.y -= (yLowerBound < 0) ? yLowerBound : 0;

        Vector2Int dimensions = Vector2Int.one;
        dimensions.x += xUpperBound + Mathf.Abs(xLowerBound);
        dimensions.y += yUpperBound + Mathf.Abs(yLowerBound);

        parent.constraintCount = dimensions.x;

        GameObject[,] temporaryGrid = new GameObject[dimensions.x, dimensions.y];

        char[,] temporaryPreviewGrid = CrosswordAlgorithm.GetEmptyCharacterLayout(dimensions);

        foreach (WordVector wordVector in _gameLayout.WordVectors)
        {
            WordTile wordTile = WordTile.CreateWordTile(wordVector.WordHintPair);
            wordTiles.Add(wordTile);
            wordTile.SetupTile(this);
            _wordConnectManager.StateUpdated += wordTile.GameStateUpdated;

            for (int i = 0; i < wordVector.OccupiedPositions.Count; i++)
            {
                Vector2Int position = wordVector.OccupiedPositions[i];
                Vector2Int newPosition = new Vector2Int(position.x + vectorOffset.x, position.y + vectorOffset.y);

                if (temporaryGrid[newPosition.x, newPosition.y] != null)
                {
                    LetterTile overlappingTile = temporaryGrid[newPosition.x, newPosition.y].GetComponent<LetterTile>();
                    WordTile overlappingWordTile = wordTiles.Find(x => x.ContainsLetterTile(overlappingTile));

                    wordTile.AddLetter(overlappingTile);

                    wordTile.ConnectOverlappingWordTile(overlappingTile, overlappingWordTile);
                    overlappingWordTile.ConnectOverlappingWordTile(overlappingTile, wordTile);
                    continue;
                }

                GameObject tile = Instantiate(_letterTile, parent.transform);
                tile.name = $"letter [{newPosition.x}, {newPosition.y}]";

                LetterTile letterTile = tile.GetComponent<LetterTile>();
                letterTile.UpdateColors();
                letterTile.LetterDisplay.text = wordVector.WordHintPair.Word[i].ToString().ToUpper();

                letterTiles.Add(letterTile);
                wordTile.AddLetter(letterTile);

                temporaryGrid[newPosition.x, newPosition.y] = tile;
                temporaryPreviewGrid[newPosition.x, newPosition.y] = wordVector.WordHintPair.Word[i];
            }

            for (int i = 0; i < wordVector.AffectedPositions.Count; i++)
            {
                Vector2Int position = wordVector.AffectedPositions[i];
                int xPos = position.x + vectorOffset.x;
                int yPos = position.y + vectorOffset.y;

                if (dimensions.x > xPos && xPos >= 0 && dimensions.y > yPos && yPos >= 0)
                    if (temporaryPreviewGrid[xPos, yPos] == '\0')
                        temporaryPreviewGrid[xPos, yPos] = '#';
            }
        }

        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = dimensions.y - 1; y >= 0; y--)
            {
                if (temporaryGrid[x, y] == null)
                {
                    GameObject tile = Instantiate(_blockedTile, parent.transform);
                    tile.GetComponent<Image>().color = new Color();
                    tile.name = $"blocked [{x}, {y}]";
                    temporaryGrid[x, y] = tile;
                }
                else
                {
                    temporaryGrid[x, y].transform.SetAsLastSibling();
                }
            }
        }

        _gameLayout.SetGridTiles(temporaryGrid);
        _gameLayout.SetGridLetters(temporaryPreviewGrid);
    }

}
