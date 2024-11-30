using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Unpause();
        }
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void ToMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        LevelController.DestroyController();
        SceneManager.LoadScene("MainMenu");
    }
}
