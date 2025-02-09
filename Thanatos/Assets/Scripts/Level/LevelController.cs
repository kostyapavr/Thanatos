using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static int aliveEnemies = 0;
    public static float playerHp = 0;
    public static bool playerHasBow;
    public static bool playerHasSword;
    public static bool playerHasHelmet;
    public static bool isBossLevel;
    public static bool playerIsGod = false;
    public static int playerSelectedWeapon = 0;
    public static IPickupableWeapon currentPlayerWeapon;

    public static UnityEvent enemyDeathEvent = new UnityEvent();
    public static UnityEvent enemySpawnEvent = new UnityEvent();
    public static UnityEvent playerHpEvent = new UnityEvent();
    public static UnityEvent playerArrowShootEvent = new UnityEvent();
    public static UnityEvent playerPickupItemEvent = new UnityEvent();
    public static UnityEvent bossDeathEvent = new UnityEvent();

    void Awake()
    {
        enemyDeathEvent.AddListener(EnemyDeath);
        enemySpawnEvent.AddListener(EnemySpawned);
        playerHpEvent.AddListener(PlayerTookDamage);
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += CheckScene;
    }

    void CheckScene(Scene arg0, LoadSceneMode arg1)
    {
        string sceneName = arg0.name;
        if (sceneName == "MainMenu" || sceneName == "Entrance")
        {
            aliveEnemies = 0;
            playerHp = 0;
            playerHasBow = false;
            playerHasSword = false;
            playerHasHelmet = false;
            isBossLevel = false;
        }
    }

    void EnemyDeath()
    {
        if (aliveEnemies > 0) aliveEnemies -= 1;
        else Debug.Log("Error: Negative aliveEnemies");
    }

    void EnemySpawned()
    {
        aliveEnemies += 1;
    }

    void PlayerTookDamage()
    {
        playerHp = FindObjectOfType<Player>().currentHealth;
    }

    public static void SetBossLevel()
    {
        isBossLevel = true;
    }

    public static void SetNormalLevel()
    {
        isBossLevel = false;
    }

    public static void DestroyController()
    {
        Destroy(GameObject.Find("LevelController"));
    }
}
