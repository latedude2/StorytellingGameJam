using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] songs;
    [SerializeField] AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio.clip = songs[0];
        audio.Play(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(!audio.isPlaying)
          playRandomMusic();
    }

    void playRandomMusic() {
        audio.clip = songs[Random.Range(0,songs.Length)] as AudioClip;
        audio.Play();
     }
}
