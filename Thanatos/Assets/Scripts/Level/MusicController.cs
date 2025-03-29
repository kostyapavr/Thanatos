using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public AudioClip menuClip;
    public AudioClip level1;
    public AudioClip level2;
    public AudioClip level3;
    public AudioSource audioSource;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += CheckScene;
    }

    void CheckScene(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex <= 1 && audioSource.clip != menuClip)
        {
            audioSource.Stop();
            audioSource.clip = menuClip;
            audioSource.Play();
        }
        else if (arg0.buildIndex > 1 && arg0.buildIndex <= 6 && audioSource.clip != level1)
        {
            audioSource.Stop();
            audioSource.clip = level1;
            audioSource.Play();
        }
        else if (arg0.buildIndex > 6 && arg0.buildIndex <= 11 && audioSource.clip != level2)
        {
            audioSource.Stop();
            audioSource.clip = level2;
            audioSource.Play();
        }
        else if (arg0.buildIndex > 11 && audioSource.clip != level3)
        {
            audioSource.Stop();
            audioSource.clip = level3;
            audioSource.Play();
        }
    }
}
