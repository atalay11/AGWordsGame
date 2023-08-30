using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WordSpyGameData
{
    public int highestScore;
}

public class PersistentDataManager : GenericSingleton<PersistentDataManager>
{
    WordSpyDatabase m_wordSpyDatabase;

    protected override void AwakeImpl()
    {
        m_wordSpyDatabase = new WordSpyDatabase();
    }

    public void Increase()
    {
        var gameData = m_wordSpyDatabase.LoadGameData();
        Debug.Log($"Higest Score: {gameData.highestScore}");
        gameData.highestScore += 1;
        m_wordSpyDatabase.SaveData(gameData);
    }
}



class WordSpyDatabase
{
    private readonly string m_path = Application.persistentDataPath + Path.DirectorySeparatorChar + "wordSpy.json";

    public void SaveData(WordSpyGameData data)
    {
        CreateDefaultFileIfNotExists();

        string gameDataString = JsonUtility.ToJson(data);
        File.WriteAllText(m_path, gameDataString);
    }

    public WordSpyGameData LoadGameData()
    {
        CreateDefaultFileIfNotExists();

        string gameDataString = File.ReadAllText(m_path);
        return JsonUtility.FromJson<WordSpyGameData>(gameDataString);
    }

    private void CreateDefaultFileIfNotExists()
    {
        if (!File.Exists(m_path))
        {
            File.CreateText(m_path).Dispose();
            var gameDataString = JsonUtility.ToJson(InitialWordSpyGameData());
            Debug.Log($"writing to the file, gameDataString: {gameDataString}");
            File.WriteAllText(m_path, gameDataString);
        }
    }

    private WordSpyGameData InitialWordSpyGameData()
    {
        WordSpyGameData wordSpyGameData = new WordSpyGameData();
        wordSpyGameData.highestScore = 1;

        return wordSpyGameData;
    }
}

