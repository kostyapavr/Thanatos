using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ChangeVolume : MonoBehaviour
{
    public AudioMixer mixer;

    private float musicVolume = 1f;

    private void Start()
    {
        LoadVolume();
        Application.quitting += SaveVolume;
        SceneManager.sceneLoaded += SaveVolume;
    }

    public void Change(float value)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
        musicVolume = value;
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    private void SaveVolume(Scene arg0, LoadSceneMode arg1)
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
    }
}
