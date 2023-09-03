using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ArcadeWordSpyGameData
{
    public long highestScore;
}

public class PersistanceManager : GenericSingleton<PersistanceManager>
{
    ArcadeWordSpyDatabase m_arcadeWordSpyDatabase;

    protected override void AwakeImpl()
    {
        m_arcadeWordSpyDatabase = new ArcadeWordSpyDatabase();
    }

    public ArcadeWordSpyGameData GetArcadeWordSpyGameData()
    {
        return m_arcadeWordSpyDatabase.LoadGameData();
    }

    public void SetArcadeWordSpyGameData(ArcadeWordSpyGameData gameData)
    {
        m_arcadeWordSpyDatabase.SaveData(gameData);
    }
}

class GameDatabaseUtils
{
    static public void SaveData<T>(string path, T data)
    {
        string gameDataString = JsonUtility.ToJson(data);
        File.WriteAllText(path, gameDataString);
    }

    static public T LoadData<T>(string path)
    {
        string gameDataString = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(gameDataString);
    }

    // returns true if file is created, false otherwise
    static public bool CreateFile(string path)
    {
        if (!File.Exists(path))
        {
            File.CreateText(path).Dispose();
            return true;
        }

        return false;
    }
}

class ArcadeWordSpyDatabase
{
    public void SaveData(ArcadeWordSpyGameData data)
    {
        CreateDefaultFileIfNotExists();

        GameDatabaseUtils.SaveData(m_path, data);
    }

    public ArcadeWordSpyGameData LoadGameData()
    {
        CreateDefaultFileIfNotExists();

        return GameDatabaseUtils.LoadData<ArcadeWordSpyGameData>(m_path);
    }

    private void CreateDefaultFileIfNotExists()
    {
        if (GameDatabaseUtils.CreateFile(m_path))
        {
            Debug.Log($"Creating WordSpyDatabase file.");
            GameDatabaseUtils.SaveData(m_path, InitialWordSpyGameData());
        }
    }

    private ArcadeWordSpyGameData InitialWordSpyGameData()
    {
        return new ArcadeWordSpyGameData { highestScore = 0 };
    }

    // -- 
    private readonly string m_path = Application.persistentDataPath + Path.DirectorySeparatorChar + "arcadeWordSpy.json";

}

