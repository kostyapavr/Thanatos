using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public AudioClip menuClip;
    public AudioClip level1;
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
            audioSource.clip = menuClip;
            audioSource.Play();
        }
        else if (arg0.buildIndex > 1 && audioSource.clip != level1)
        {
            audioSource.clip = level1;
            audioSource.Play();
        }
    }
}
