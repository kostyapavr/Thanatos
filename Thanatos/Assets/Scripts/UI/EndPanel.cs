using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanel : MonoBehaviour
{
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        LevelController.DestroyController();
        SceneManager.LoadScene("MainMenu");
    }
}
