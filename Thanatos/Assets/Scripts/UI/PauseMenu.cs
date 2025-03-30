using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    [HideInInspector]
    public bool isInSettings = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInSettings) CloseSettings();
            else Unpause();
        }
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        isInSettings = false;
    }

    public void ToMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        LevelController.DestroyController();
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        isInSettings = true;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        isInSettings = false;
    }
}
