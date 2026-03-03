using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.5f, 1.5f)] public float pitchMin = 0.95f;
        [Range(0.5f, 1.5f)] public float pitchMax = 1.05f;
        public bool loop = false;

        [HideInInspector] public AudioSource source;
    }

    [Header("Sound Effects")]
    public Sound[] sfx;

    [Header("Music")]
    public Sound[] music;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSounds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSounds()
    {
        // Create AudioSource components for each sound
        foreach (Sound s in sfx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    // Play sound effect with random pitch variation
    public void PlaySFX(string soundName)
    {
        Sound s = System.Array.Find(sfx, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + soundName);
            return;
        }

        s.source.pitch = Random.Range(s.pitchMin, s.pitchMax);
        s.source.PlayOneShot(s.clip, s.volume);
    }

    // Play music (looping)
    public void PlayMusic(string musicName)
    {
        Sound s = System.Array.Find(music, sound => sound.name == musicName);
        if (s == null)
        {
            Debug.LogWarning("Music not found: " + musicName);
            return;
        }

        // Stop other music first
        foreach (Sound m in music)
        {
            if (m.source.isPlaying)
                m.source.Stop();
        }

        s.source.Play();
    }

    // Stop music
    public void StopMusic(string musicName)
    {
        Sound s = System.Array.Find(music, sound => sound.name == musicName);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    // Play random SFX from array (no consecutive repeats)
    private int lastRandomIndex = -1;
    public void PlayRandomSFX(string[] soundNames)
    {
        if (soundNames == null || soundNames.Length == 0) return;

        int index;
        do
        {
            index = Random.Range(0, soundNames.Length);
        } while (index == lastRandomIndex && soundNames.Length > 1);

        lastRandomIndex = index;
        PlaySFX(soundNames[index]);
    }
}
