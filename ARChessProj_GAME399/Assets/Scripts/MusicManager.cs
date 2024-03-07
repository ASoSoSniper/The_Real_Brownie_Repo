using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicClips;
    private AudioSource musicSource;

    private float currClipLength;
    private int currClipIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        Invoke("PlayNextSong", currClipLength);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayNextSong()
    {
        musicSource.clip = musicClips[currClipIndex];
        currClipLength = musicClips[currClipIndex].length;
        musicSource.Play();
        currClipIndex++;

        if (currClipIndex >= musicClips.Length) currClipIndex = 0;        

        Invoke("PlayNextSong", musicClips[currClipIndex].length);
    }
}
