using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private Sound[] sounds;

    float SongVolumePercent = .6f;
    int activeSongSourceIndex;
    AudioSource[] songSources;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            songSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newSongSource = new GameObject("Song Source " + (i + 1));
                songSources[i] = newSongSource.AddComponent<AudioSource>();
                newSongSource.transform.parent = transform;
            }

            foreach (Sound s in sounds)
            {
                s.audioSource = gameObject.AddComponent<AudioSource>();
                s.audioSource.clip = s.audioClip;
                s.audioSource.volume = s.volume;
                s.audioSource.pitch = s.pitch;
            }

        }
    }

    // to play short sound (sound effect etc...)
    // not useful for music cause cant change the volume of clip while its playing it should be Audio Source
    public void playAudio(AudioClip audioClip, Vector3 audioPos, float audioVolumePercent = 1) {
        if (audioClip != null)
        {
            AudioSource.PlayClipAtPoint(audioClip, audioPos, audioVolumePercent);
        }
    }

    public void playAudio(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.audioSource.Play();
        }
    }

    public void playSong(AudioClip audioClip, float fadeTime = 2) {
        activeSongSourceIndex = 1 - activeSongSourceIndex;
        songSources[activeSongSourceIndex].clip = audioClip;
        songSources[activeSongSourceIndex].Play();

        StartCoroutine(fadeSong(fadeTime));
    }

    IEnumerator fadeSong(float fadeTime) {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeTime;
            songSources[activeSongSourceIndex].volume = Mathf.Lerp(0, SongVolumePercent, percent);
            songSources[1 - activeSongSourceIndex].volume = Mathf.Lerp(SongVolumePercent, 0, percent);
            yield return null;
        }
    }


    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip audioClip;

        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch;
        public bool loop;

        [HideInInspector] public AudioSource audioSource;
    }
}
