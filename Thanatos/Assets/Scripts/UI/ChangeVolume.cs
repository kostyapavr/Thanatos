using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider soundSlider;
    private float musicVolume = 1f;
    private float soundVolume = 1f;

    private void Start()
    {
        LoadVolume();
        Application.quitting += SaveVolume;
        SceneManager.sceneLoaded += SaveVolume;
    }

    public void ChangeMusic(float value)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
        musicVolume = value;
    }

    public void ChangeSound(float value)
    {
        mixer.SetFloat("SoundVol", Mathf.Log10(value) * 20);
        soundVolume = value;
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SoundVolume", musicVolume);
        PlayerPrefs.Save();
    }

    private void SaveVolume(Scene arg0, LoadSceneMode arg1)
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SoundVolume", musicVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
    }
}
