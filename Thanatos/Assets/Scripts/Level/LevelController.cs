using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static int maxArmorHP = 30;
    public static int maxHelmetHP = 35;

    public static int aliveEnemies = 0;
    public static float playerHp = 0;
    public static bool playerHasBow;
    public static bool playerHasApolloBow;
    public static bool playerHasErosBow;
    public static bool playerHasSword;
    public static bool playerHasHarpeSword;
    public static bool playerHasPeleusSword;
    public static bool playerHasHelmet;
    public static bool playerHasAchillesHelmet;
    public static bool playerHasAchillesArmor;
    public static bool playerHasArmor;
    public static bool playerHasFireBow;
    public static bool playerHasShield = false;
    public static bool playerShieldActive = false;
    public static bool isBossLevel;
    public static bool playerIsGod = false;
    public static List<IPickupableWeapon> playerWeapons = new List<IPickupableWeapon>();
    public static IPickupableWeapon currentPlayerWeapon;
    public static bool isNormalDifficulty = true;
    public static int helmetHP = maxHelmetHP;
    public static int armorHP = maxArmorHP;
    public static int lavaBootsHP = 40;
    public static BonusTypes playerBonusType = BonusTypes.None;
    public static bool playerHasLavaBoots = false;
    public static int playerBoostHp = 0;
    public static bool playerEquippedDefaultSword = false;
    public static bool playerEquippedPeleusSword = false;
    public static bool playerEquippedHarpeSword = false;

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
            playerHasFireBow = false;
            playerHasBow = false;
            playerHasSword = false;
            playerHasShield = false;
            playerHasHelmet = false;
            playerHasAchillesArmor = false;
            playerHasAchillesHelmet = false;
            playerHasApolloBow = false;
            playerHasErosBow = false;
            playerHasPeleusSword = false;
            playerHasHarpeSword = false;
            playerHasArmor = false;
            playerHasLavaBoots = false;
            isBossLevel = false;
            isNormalDifficulty = true;
            playerBoostHp = 0;
            playerBonusType = BonusTypes.None;
        }
        aliveEnemies = 0;
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

        if (playerHasHelmet || playerHasAchillesHelmet)
        {
            helmetHP--;
            if (helmetHP <= 0)
            {
                if (playerHasHelmet)
                {
                    FindObjectOfType<Player>().GetComponent<PlayerMovement>().helmet.gameObject.SetActive(false);
                    playerHasHelmet = false;
                }
                else if (playerHasAchillesHelmet)
                {
                    FindObjectOfType<Player>().RemoveAchillesHelmet();
                    playerHasAchillesHelmet = false;
                }
            }
        }
        if (playerHasArmor || playerHasAchillesArmor)
        {
            armorHP--;
            if (armorHP <= 0)
            {
                if (playerHasAchillesArmor)
                {
                    FindObjectOfType<Player>().RemoveAchillesArmor();
                    playerHasAchillesArmor = false;
                }
                else if (playerHasArmor)
                {
                    FindObjectOfType<Player>().RemoveArmor();
                    playerHasArmor = false;
                }
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
