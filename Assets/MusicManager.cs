using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] songs;
    public float volume;
    private float trackTime;
    private int songsPlayed;
    private bool[] beenPlayed;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        beenPlayed = new bool[songs.Length];

        if (!audioSource.isPlaying)
            ChangeSong(Random.Range(0, songs.Length));
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volume;

        if (audioSource.isPlaying)
            trackTime += 1 * Time.deltaTime;
        if(!audioSource.isPlaying || trackTime >= audioSource.clip.length || Input.GetKeyDown(KeyCode.Space))
            ChangeSong(Random.Range(0, songs.Length));

        ResetShuffle();
    }

    public void ChangeSong(int songPicked)
    {
        if (!beenPlayed[songPicked])
        {
            trackTime = 0;
            songsPlayed++;
            beenPlayed[songsPlayed] = true;
            audioSource.clip = songs[songPicked];
            audioSource.Play();
        }
        else
            audioSource.Stop();
    }

    private void ResetShuffle()
    {
        if (songsPlayed == songs.Length)
        {
            songsPlayed = 0;
            for (int i = 0; i < songs.Length; i++)
            {
                beenPlayed[i] = false;
            }
        }
    }
}
