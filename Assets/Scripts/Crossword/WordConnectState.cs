using System.Collections.Generic;
public class WordConnectState
{
    public List<LetterInput> CurrentWordInput { get; private set; }

    public List<string> WordsInCrossword { get; private set; }

    public List<string> CorrectlyAddedWords { get; private set; }



    public int WordHintBalance { get; private set; }

    public int LetterHintBalance { get; private set; }

    public int DescriptiveHintBalance { get; private set; }

    public int CurrentScore { get; private set; } = 0;

    public int CurrentStreakBonusScore { get; private set; } = 0;

    public int CorrectAnswerStreak { get; private set; } = 0;

    public WordConnectState()
    {
        CurrentWordInput = new List<LetterInput>();
        WordsInCrossword = new List<string>();
        CorrectlyAddedWords = new List<string>();
    }

    public void AddLetterInput(LetterInput letter) => CurrentWordInput.Add(letter);

    public void RemoveLastLetter() => CurrentWordInput.RemoveAt(CurrentWordInput.Count - 1);

    public void SetAvailableWords(List<string> availableWords) => WordsInCrossword = availableWords;

    public void AddScore(int amount) => CurrentScore += amount;

    public void AddFoundWord(string foundWord) => CorrectlyAddedWords.Add(foundWord);

    public void ClearInput() => CurrentWordInput.Clear();

    public string GetCurrentWordInput()
    {
        string sequence = string.Empty;

        foreach (LetterInput letterInput in CurrentWordInput)
            sequence += letterInput.letter;

        return sequence.ToUpper();
    }
}
