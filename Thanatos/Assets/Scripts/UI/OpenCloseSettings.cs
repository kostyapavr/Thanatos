using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject Panel;
    public GameObject mainArt;

    public void OpenPanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(true);
            mainArt.SetActive(false);
        }
    }
    public void ClosePanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
            mainArt.SetActive(true);
        }
    }
}
