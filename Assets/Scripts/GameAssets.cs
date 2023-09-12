using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets m_instance;

    public static GameAssets Instance
    {
        get {
            if (m_instance == null)
                m_instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return m_instance;
        }
    }

    public SoundAudioClip[] soundAudioClipList;

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
