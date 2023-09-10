using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class WordConnectManager : MonoBehaviour
{
    public static WordConnectManager Instance { get; set; }

    public WordConnectConfigurationData Configuration { get; private set; }

    public WordConnectState WordConnectState { get; private set; }

    public List<LetterInput> AvailableLetters { get; private set; }

    public Stopwatch GameTimer { get; private set; }

    public bool IsPaused { get; private set; }

    public bool IsGameActive { get; private set; }

    public Action<WordConnectState> StateUpdated;

    public event Action Initialized;

    public event Action BuildGame;

    public event Action Started;

    public event Action Cleanup;

    private void Awake()
    {
        Instance = this; // Singleton safety
        GameTimer = new Stopwatch();
    }

    public void StartGame(WordConnectConfigurationData config)
    {
        if (IsGameActive)
            return;

        IsGameActive = true;
        IsPaused = false;
        GameTimer.Restart();
        Configuration = config;

        WordConnectState = new WordConnectState();

        List<string> wordsInCrossword = new List<string>();

        foreach (WordVector wordVector in Configuration.WordVectors)
        {
            wordsInCrossword.Add(wordVector.WordHintPair.Word);
        }

        WordConnectState.SetAvailableWords(wordsInCrossword);

        List<char> availableLetters = GetAllLettersFromWords(WordConnectState.WordsInCrossword);
        SetAvailableLetters(availableLetters);

        Initialized?.Invoke();
        BuildGame?.Invoke();
        Started?.Invoke();
    }


    public void Finish()
    {
        if (!IsGameActive)
            return;

        Stop();

        float timeElapsed = (float)GameTimer.Elapsed.TotalSeconds;

        int scoreEarned = WordConnectState.CurrentScore + WordConnectState.CurrentStreakBonusScore + (int)Configuration.CalculatePoints(timeElapsed);

        int maxPossibleScore = Configuration.ScorePerWord * WordConnectState.WordsInCrossword.Count + 456; // Time related
    }

    public void Stop()
    {
        IsGameActive = false;
        IsPaused = false;
        GameTimer.Stop();

        Cleanup?.Invoke();
    }

    public void Restart()
    {
        Stop();
        StartGame(Configuration);
    }

    public void HandleLetterInput(LetterInput letterInput)
    {
        if (!char.IsLetter(letterInput.letter)) return;

        List<LetterInput> currentLetterSequence = WordConnectState.CurrentWordInput;

        if (currentLetterSequence.Count > 1 && currentLetterSequence[currentLetterSequence.Count - 2].id == letterInput.id)
        {
            WordConnectState.RemoveLastLetter();
            StateUpdated?.Invoke(WordConnectState);
        }
        else if (!WordConnectState.CurrentWordInput.Contains(letterInput))
        {
            WordConnectState.AddLetterInput(letterInput);
            StateUpdated?.Invoke(WordConnectState);
        }
    }


    public void SubmitWord()
    {
        if (WordConnectState == null || WordConnectState.CurrentWordInput == null || WordConnectState.CurrentWordInput.Count == 0) return;

        string spelledWord = WordConnectState.GetCurrentWordInput().ToLower();

        if (WordConnectState.WordsInCrossword.Contains(spelledWord, StringComparer.OrdinalIgnoreCase))
        {
            if (WordConnectState.CorrectlyAddedWords.Contains(spelledWord, StringComparer.OrdinalIgnoreCase))
            {
                // Word has already found
            }
            else
            {
                WordConnectState.AddFoundWord(spelledWord);
                WordConnectState.AddScore(Configuration.ScorePerWord);
            }
        }
        else
        {
            // Invalid
        }

        WordConnectState.ClearInput();
        StateUpdated?.Invoke(WordConnectState);

        if (CheckCompletion()) // All words found
            Finish();
    }

    private bool CheckCompletion()
    {
        if (WordConnectState.CorrectlyAddedWords.Count == 0)
            return false;

        List<string> wordsInCrossword = WordConnectState.WordsInCrossword;

        foreach (string word in WordConnectState.CorrectlyAddedWords)
            wordsInCrossword.Remove(word);

        return wordsInCrossword.Count == 0;
    }

    private List<char> GetAllLettersFromWords(List<string> words)
    {
        List<char> neededLetters = new List<char>();

        foreach (char letter in words[0])
            neededLetters.Add(letter);

        for (int i = 1; i < words.Count; i++)
        {
            foreach (char letter in words[i])
            {
                if (neededLetters.Contains(letter))
                {
                    int unaddedLetterOccurrences = words[i].Count(letterInWord => (letterInWord == letter));
                    int addedLetterOccurrences = neededLetters.Count(addedLetter => (addedLetter == letter));

                    if (unaddedLetterOccurrences > addedLetterOccurrences)
                    {
                        for (int l = 0; l < unaddedLetterOccurrences - addedLetterOccurrences; l++)
                            neededLetters.Add(letter);
                    }
                }
                else
                {
                    neededLetters.Add(letter);
                }
            }
        }
        return neededLetters;
    }

    private void SetAvailableLetters(List<char> letters)
    {
        AvailableLetters = new List<LetterInput>();

        for (int i = 0; i < letters.Count; i++)
        {
            AvailableLetters.Add(new LetterInput(i, letters[i]));
        }
    }
}
