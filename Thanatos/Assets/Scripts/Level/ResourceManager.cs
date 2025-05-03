using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public int enemiesToSpawnMin;
    public int enemiesToSpawnMax;
    [HideInInspector] public int enemiesToSpawn;
    public int playerArrowsToGive;
    public int playerArrowsToGive_Hard;
    public int maxPlayerHP;
    public int maxPlayerHP_Hard;
    public int rocksToSpawn;

    [HideInInspector] public int MaxPlayerHP {  
        get 
        { 
            int baseHp = LevelController.isNormalDifficulty ? maxPlayerHP : maxPlayerHP_Hard;
            if (LevelController.playerBoostHp > 0) baseHp += LevelController.playerBoostHp;
            return baseHp;
        }  
    }

    [HideInInspector]
    public int PlayerArrowsToGive
    {
        get { return LevelController.isNormalDifficulty ? playerArrowsToGive : playerArrowsToGive_Hard; }
    }

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
