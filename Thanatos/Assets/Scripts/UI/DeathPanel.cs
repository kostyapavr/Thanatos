using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    public void ReturnToEntrance()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Entrance");
    }
}
