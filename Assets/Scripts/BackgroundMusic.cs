using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;
    [HideInInspector] public AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", audioSource.volume);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static void ChangeVolume(float volume)
    {
        instance.audioSource.volume = volume;
    }
}
