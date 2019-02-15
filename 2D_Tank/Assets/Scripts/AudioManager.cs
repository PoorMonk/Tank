using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;
    private AudioSource m_audioSource;

    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        m_audioSource.clip = clip;
        if (!m_audioSource.isPlaying)
        {
            m_audioSource.Play();
        }
        
    }
}
