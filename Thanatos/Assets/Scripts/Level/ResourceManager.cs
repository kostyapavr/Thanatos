using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public int enemiesToSpawnMin;
    public int enemiesToSpawnMax;
    [HideInInspector] public int enemiesToSpawn;
    public int playerArrowsToGive;
    public int maxPlayerHP;
    public int rocksToSpawn;

    void Awake()
    {
        if (Instance == null)
        {
            enemiesToSpawn = Random.Range(enemiesToSpawnMin, enemiesToSpawnMax+1);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
