using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeResalution : MonoBehaviour
{
    public TMP_Dropdown Dropdown;

    public void Change()
    {
        if(Dropdown.value == 0)
        {
            Screen.SetResolution(800, 600, true);
        }
        else if (Dropdown.value == 1)
        {
           Screen.SetResolution(1024, 768, true);
        }
        else if (Dropdown.value == 2)
        {
            Screen.SetResolution(1366, 768, true);
        }
        else if (Dropdown.value == 3)
        {
            Screen.SetResolution(1920, 1080, true);
        }
    }
}
