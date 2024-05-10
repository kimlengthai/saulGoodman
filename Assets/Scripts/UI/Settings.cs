using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;

    [SerializeField] Image musicImage;
    [SerializeField] Image soundImage;

    [SerializeField] Sprite musicIcon;
    [SerializeField] Sprite musicMuteIcon;
    [SerializeField] Sprite soundIcon;
    [SerializeField] Sprite soundMuteIcon;

    public void Start()
    {
        Time.timeScale = 0;

        if (BackgroundMusic.instance != null)
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", BackgroundMusic.instance.audioSource.volume);

        Game.audioMixer.GetFloat("Volume", out float soundVolume);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", (soundVolume + 80f) / 100f);

        VolumeMusicChanged();
        VolumeSoundChanged();
    }


    public void MuteMusic()
    {
        musicSlider.value = 0;
        VolumeMusicChanged();
    }


    public void VolumeMusicChanged()
    {
        if (BackgroundMusic.instance != null)
            BackgroundMusic.ChangeVolume(musicSlider.value);
        musicImage.sprite = musicSlider.value == 0 ? musicMuteIcon : musicIcon;
    }


    public void MuteSound()
    {
        soundSlider.value = 0;
        VolumeSoundChanged();
    }


    public void VolumeSoundChanged()
    {
        soundImage.sprite = soundSlider.value == 0 ? soundMuteIcon : soundIcon;
        Game.audioMixer.SetFloat("Volume", 100f*soundSlider.value - 80f);
    }


    public void Close()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);

        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("Settings");
    }
}
