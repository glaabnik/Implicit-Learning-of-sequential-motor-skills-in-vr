using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Audio players components.
    public AudioSource hitSource;
    public AudioSource backgroundSource;
    public AudioClip[] hitSoundsAccuracy;
    public AudioClip backgroundMusic;
    public AudioClip backgroundMusic2;
    public AudioClip backgroundMusic3;
    public float volumeBackGroundMusic = 0.1f;

    // Random pitch adjustment range.
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    // Singleton instance.
    public static SoundManager Instance = null;

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if(!backgroundSource.isPlaying)
        {
            playRandomBackgroundMusic();
        }
    }

    // Play a single clip through the sound effects source.
    public void PlayHitSound(int number, float volume)
    {
        hitSource.PlayOneShot(hitSoundsAccuracy[number], volume);
    }

    // Play a single clip through the music source.
    public void PlayBackgroundMusic()
    {
        //backgroundSource.PlayOneShot(backgroundMusic, volumeBackGroundMusic);
        playRandomBackgroundMusic();
    }

    private void playRandomBackgroundMusic()
    {
        int z = Random.Range(0, 2);
        AudioClip nextBackgrondMusic = null;
        if (z == 0) nextBackgrondMusic = backgroundMusic;
        if (z == 1) nextBackgrondMusic = backgroundMusic2;
        if (z == 2) nextBackgrondMusic = backgroundMusic3;

        backgroundSource.PlayOneShot(nextBackgrondMusic, volumeBackGroundMusic);
    }


    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomHitEffect()
    {
        int randomIndex = Random.Range(0, hitSoundsAccuracy.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        hitSource.pitch = randomPitch;
        hitSource.clip = hitSoundsAccuracy[randomIndex];
        hitSource.Play();
    }

}