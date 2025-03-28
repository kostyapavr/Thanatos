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
    public static bool playerHasArmor;
    public static bool isBossLevel;
    public static bool playerIsGod = false;
    public static List<IPickupableWeapon> playerWeapons = new List<IPickupableWeapon>();
    public static IPickupableWeapon currentPlayerWeapon;
    public static bool isNormalDifficulty = true;
    public static int helmetHP = 20;
    public static int armorHP = 15;
    public static BonusTypes playerBonusType = BonusTypes.None;
    public static bool playerHasLavaBoots = false;

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
            isNormalDifficulty = true;
        }

        if (sceneName.Contains("Boss")) SetBossLevel();
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

        if (playerHasHelmet)
        {
            helmetHP--;
            if (helmetHP <= 0)
            {
                FindObjectOfType<Player>().GetComponent<PlayerMovement>().helmet.gameObject.SetActive(false);
                playerHasHelmet = false;
            }
        }
        if (playerHasArmor)
        {
            armorHP--;
            if (armorHP <= 0)
            {
                FindObjectOfType<Player>().RemoveArmor();
                playerHasArmor = false;
            }
        }
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

    public static void ApplyBonus(BonusTypes bonusType)
    {
        playerBonusType = bonusType;

        if (bonusType == BonusTypes.AriadneThread)
        {
            GameObject.FindGameObjectWithTag("ExitPortal").GetComponent<Portal>().OpenPortal();
        }
    }

    public static void NextWeapon()
    {

    }

    public static void SelectWeapon(int ind)
    {
        int arrInd = 0;
        if (ind == 1) arrInd = playerWeapons.FindIndex(x => x.Name.Contains("Bow"));
        else if (ind == 2) arrInd = playerWeapons.FindIndex(x => x.Name.Contains("Sword"));
        else if (ind == 3) arrInd = playerWeapons.FindIndex(x => x.Name.Contains("Shield"));
        currentPlayerWeapon = playerWeapons[arrInd];
    }
}
