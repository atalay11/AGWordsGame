using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{

    public enum Sound
    {
        SelectLetterCube
    }

    public static void PlaySound(Sound sound)
    {
        InitializeOneShotSound();   
        m_oneShotAudioClip.PlayOneShot(GetAudioClip(sound));
    }

    private static void InitializeOneShotSound()
    {
        if (m_oneShotSoundGameObject == null)
        {
            GameObject m_oneShotSoundGameObject = new GameObject("One Shot Sound");
            m_oneShotAudioClip = m_oneShotSoundGameObject.AddComponent<AudioSource>();
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.Instance.soundAudioClipList)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    private static GameObject m_oneShotSoundGameObject;
    private static AudioSource m_oneShotAudioClip;
    
}
