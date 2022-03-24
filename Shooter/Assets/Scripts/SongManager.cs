using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] private AudioClip gameSong;
    [SerializeField] private AudioClip menuSong;

    private void Start()
    {
        AudioManager.instance.playSong(menuSong, 2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.playSong(gameSong, 3);
        }
    }
}
