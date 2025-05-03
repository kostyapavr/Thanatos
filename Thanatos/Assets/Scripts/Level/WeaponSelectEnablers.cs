using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectEnablers : MonoBehaviour
{
    public List<GameObject> weaponTables;

    private void Start()
    {
        foreach (GameObject weaponTable in weaponTables)
        {
            if (PlayerPrefs.GetInt(weaponTable.name, 0) != 0) weaponTable.SetActive(true);
        }
    }
}
