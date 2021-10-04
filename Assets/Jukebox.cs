using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{

    private AudioSource aud;

    private void Start()
    {
        aud = GameObject.Find("MusicSource").GetComponent<AudioSource>();
    }

    public void StopSong()
    {
        aud.Stop();
    }

    public void PlaySong(AudioClip s)
    {
        StopSong();
        aud.clip = s;
        aud.Play();
    }
}
