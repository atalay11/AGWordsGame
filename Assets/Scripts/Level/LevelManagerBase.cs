using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level 
{
    public Level() {}
    public Level(int level) {}

    public static Level GetNextLevel(Level level) { return new Level(level.Value + 1); }
    
    public int Value
    {
        get { return m_Level; }
        set { m_Level = value < 1 ? 1 : value; }
    }

    private int m_Level = 1;
}

public enum LevelEventType
{
    NextLevel
}

public interface ILevelManager
{
    public abstract void InitLevel(Level level);
    public void AddEventCallback(LevelEventType eventType, Action<object, EventArgs> action);
    public GameMode GetGameMode();
}

public abstract class LevelManagerBase: MonoBehaviour, ILevelManager
{
    [SerializeField] GameMode gameMode;

    public void InitLevel(Level level) { InitLevelImpl(level); }
    public GameMode GetGameMode() { return gameMode; }
    public void AddEventCallback(LevelEventType eventType, Action<object, EventArgs> action)
    {
        if (!m_EventMap.ContainsKey(eventType))
        {
            m_EventMap[eventType] = null;
        }

        m_EventMap[eventType] += new EventHandler<EventArgs>(action);
    }

    protected abstract void InitLevelImpl(Level level);

    protected void RaiseEvent(LevelEventType eventType, EventArgs eventArgs)
    {
        if (m_EventMap.ContainsKey(eventType))
        {
            m_EventMap[eventType]?.Invoke(this, eventArgs);
        }
    }

    private Dictionary<LevelEventType, EventHandler<EventArgs>> m_EventMap = new Dictionary<LevelEventType, EventHandler<EventArgs>>();
}