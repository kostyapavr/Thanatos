using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    private int arrows = 5;

    private float _currentHealth;
    private float _maxHealth;
    public float currentHealth { get => _currentHealth; set => _currentHealth = value; }
    public float maxHealth { get => _maxHealth; set => _maxHealth = value; }

    public GameObject deathPanel;
    public GameObject endPanel;
    public GameObject helmetSprite;
    public GameObject armorSprite;
    public GameObject achillesHelmetSprite;
    public GameObject achillesArmorSprite;

    public GameObject pauseMenu;

    public GameObject slowEffectSprite;
    public GameObject ambrosiaEffectSprite;
    public GameObject miasmaEffectSprite;

    public Sprite goldenArmorSprite;
    public UIController ui;
    public GameObject lavaBoots;

    public Bow bow;
    private PlayerCombat sword;
    public Shield shield;
    public SpriteRenderer defaultSwordSprite;
    public SpriteRenderer peleusSwordSprite;
    public SpriteRenderer harpeSwordSprite;
    private bool godMode = false;

    public AudioSource audioSource;
    public AudioClip bowShootClip;
    public AudioClip swordAttackClip;
    public AudioClip bonusPickupClip;
    public AudioClip deathAudioClip;
    public AudioClip teleportAudioClip;
    public AudioClip slowEffectSound;
    public AudioClip shieldHitClip;

    public GameObject fireEffectParticles;

    private float hpAddWhenNewLevel = 2f;
    private float hpAddWhenNewLevel_hard = 1f;

    [HideInInspector]
    public bool is_enemySlowEffect = false;
    private float enemySlowEffectDuration = 5.0f;
    private float enemySlowEffectTime = 0f;

    private bool miasmaEffect = false;
    private bool ambrosiaEffect = false;
    private bool goldenArmorEffect = false;
    private bool hasArmor = false;
    private bool hasLavaBoots = false;
    private bool achillesArmorEffect = false;
    private bool achillesHelmetEffect = false;
    private bool hasPeleusSword = false;
    private bool hasHarpeSword = false;

    private bool isBleeding = false;

    void Start()
    {
        maxHealth = ResourceManager.Instance.MaxPlayerHP;
        arrows = ResourceManager.Instance.PlayerArrowsToGive;
        if (!LevelController.playerHasSword && LevelController.playerHasBow) arrows += 10;

        currentHealth = LevelController.playerHp == 0 ? maxHealth : Mathf.Min(maxHealth, LevelController.playerHp + (LevelController.isNormalDifficulty ? hpAddWhenNewLevel : hpAddWhenNewLevel_hard));
        LevelController.playerHp = currentHealth;

        CheckPickup();
        LevelController.playerPickupItemEvent.AddListener(CheckPickup);

        godMode = LevelController.playerIsGod;

        sword = GetComponent<PlayerCombat>();

        if (LevelController.playerHasBow || LevelController.playerHasSword || LevelController.playerHasShield)
            SelectWeapon(LevelController.currentPlayerWeapon);

        if (LevelController.playerHasArmor) EquipArmor();
        if (LevelController.playerHasLavaBoots) EquipLavaBoots();

        PlayAudio(teleportAudioClip);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = !godMode;
            LevelController.playerIsGod = godMode;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerPrefs.SetInt("HasFireBow", 1);
            PlayerPrefs.SetInt("HasApolloBow", 1);
            PlayerPrefs.SetInt("HasErosBow", 1);
            PlayerPrefs.SetInt("HasPeleusSword", 1);
            PlayerPrefs.SetInt("HasHarpeSword", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetInt("HasFireBow", 0);
            PlayerPrefs.SetInt("HasApolloBow", 0);
            PlayerPrefs.SetInt("HasErosBow", 0);
            PlayerPrefs.SetInt("HasPeleusSword", 0);
            PlayerPrefs.SetInt("HasHarpeSword", 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BookScript bs = FindObjectOfType<BookScript>();
            if (bs == null || !bs.isOpen)
            {
                if (!pauseMenu.GetComponent<PauseMenu>().isInSettings)
                {
                    if (pauseMenu.activeSelf)
                    {
                        pauseMenu.SetActive(false);
                        Time.timeScale = 1;
                    }
                    else
                    {
                        pauseMenu.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(LevelController.playerWeapons.Where(x => x.Name.Contains("Bow")).FirstOrDefault());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWeapon(LevelController.playerWeapons.Where(x => x.Name.Contains("Sword")).FirstOrDefault());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectWeapon(LevelController.playerWeapons.Where(x => x.Name.Contains("Shield")).FirstOrDefault());
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject.FindGameObjectWithTag("ExitPortal").GetComponent<Portal>().LoadNextLevel();
        }

        if (is_enemySlowEffect)
        {
            if (enemySlowEffectTime > 0) enemySlowEffectTime -= Time.deltaTime;
            else
            {
                is_enemySlowEffect = false;
                GetComponent<PlayerMovement>().ReturnToNormalSpeed();
                slowEffectSprite.SetActive(false);
            }
        }
    }

    public void ApplyEnemySlowEffect()
    {
        is_enemySlowEffect = true;
        enemySlowEffectTime = enemySlowEffectDuration;
        slowEffectSprite.SetActive(true);
        GetComponent<PlayerMovement>().SlowDown_Effect();
        PlayAudio(slowEffectSound);
    }

    public void SelectWeapon(IPickupableWeapon weapon)
    {
        if (weapon == null) return;
        if (weapon.Name.Contains("Sword") && (LevelController.playerHasSword || LevelController.playerHasPeleusSword || LevelController.playerHasHarpeSword) && !bow.isCharging && !LevelController.playerShieldActive) SelectSword();
        else if (weapon.Name.Contains("Bow") && (LevelController.playerHasBow || LevelController.playerHasFireBow || LevelController.playerHasApolloBow || LevelController.playerHasErosBow) && !sword.isAttacking && !LevelController.playerShieldActive) SelectBow();
        else if (weapon.Name.Contains("Shield") && LevelController.playerHasShield && !bow.isCharging && !sword.isAttacking) SelectShield();
    }

    private void SelectSword()
    {
        bow.HideBow();
        if (LevelController.playerHasSword) EquipDefaultSword();
        else if (LevelController.playerHasPeleusSword) EquipPeleusSword();
        else if (LevelController.playerHasHarpeSword) EquipHarpeSword();
        shield.HideShield();
        //selectedWeapon = 1;
        LevelController.currentPlayerWeapon = LevelController.playerWeapons.Find(x => x.Name.Contains("Sword"));
    }

    private void SelectBow()
    {
        bow.ShowBow();
        if (LevelController.playerHasSword) HideDefaultSword();
        else if (LevelController.playerHasPeleusSword) HidePeleusSword();
        else if (LevelController.playerHasHarpeSword) HideHarpeSword();
        shield.HideShield();
        //selectedWeapon = 0;
        LevelController.currentPlayerWeapon = LevelController.playerWeapons.Find(x => x.Name.Contains("Bow"));
    }

    private void SelectShield()
    {
        bow.HideBow();
        if (LevelController.playerHasSword) HideDefaultSword();
        else if (LevelController.playerHasPeleusSword) HidePeleusSword();
        else if (LevelController.playerHasHarpeSword) HideHarpeSword();
        shield.ShowShield();
        //selectedWeapon = 0;
        LevelController.currentPlayerWeapon = LevelController.playerWeapons.Find(x => x.Name.Contains("Shield"));
    }

    public int GetCurrentArrows()
    {
        return arrows;
    }

    public void TakeArrow()
    {
        if (arrows > 0 && !godMode) arrows -= 1;
        LevelController.playerArrowShootEvent.Invoke();
    }

    public void AddHealth(float amount)
    {
        if (currentHealth + amount > maxHealth) currentHealth = maxHealth;
        else currentHealth += amount;
        LevelController.playerHpEvent.Invoke();
    }

    void Die()
    {
        ResetItems();
        deathPanel.SetActive(true);
        Time.timeScale = 0;
        PlayAudio(deathAudioClip);
    }

    public void TakeDamage(float damage, GameObject sender = null, DamageEffects damageEffect = DamageEffects.Nothing)
    {
        if (gameObject == sender || godMode) return;

        if (LevelController.playerShieldActive && damageEffect != DamageEffects.BypassShield && damageEffect != DamageEffects.Bleed)
        {
            PlayAudio(shieldHitClip);
            return;
        }

        if (miasmaEffect && damageEffect != DamageEffects.Bleed) damage *= 1.5f;
        if (ambrosiaEffect)
        {
            RemoveAmbrosiaEffect();
            return;
        }
        if (goldenArmorEffect)
        {
            goldenArmorEffect = false;
            ShowArmorTakeDamage();
            return;
        }
        if (achillesArmorEffect && sender != null && sender.GetComponent<MeleeEnemy>())
        {
            damage = 0.25f;
            ShowArmorTakeDamage();
        }

        if (LevelController.playerHasArmor)
        {
            damage /= 2f;
            ShowArmorTakeDamage();
        }

        if (damageEffect == DamageEffects.Bleed && !isBleeding)
        {
            isBleeding = true;
            Invoke("Bleed", 1f);
            Invoke("Bleed", 3f);
            Invoke("StopBleed", 3.1f);
        }
        if (damageEffect == DamageEffects.MeduzaBite)
        {
            bow.IncreaseDelayLength();
            PlayAudio(slowEffectSound);
        }
        if (damageEffect == DamageEffects.SetOnFire)
        {
            Instantiate(fireEffectParticles, transform);
            Invoke("FireDamage", 1f);
            Invoke("FireDamage", 2f);
            Invoke("FireDamage", 3f);
            Invoke("FireDamage", 4f);
            Invoke("FireDamage", 5f);
            bow.IncreaseDelayLength();
            GetComponent<PlayerMovement>().SetOnFire();
        }


        if (currentHealth - damage > 0)
        {
            if (damageEffect == DamageEffects.SlowDown) ApplyEnemySlowEffect();
            currentHealth -= damage;
            if (damageEffect != DamageEffects.Bleed)
            {
                LevelController.playerHpEvent.Invoke();
                ShowArmorTakeDamage();
            }
        }
        else
        {
            currentHealth = 0;
            LevelController.playerHpEvent.Invoke();
            Die();
        }
    }

    void Bleed()
    {
        TakeDamage(0.25f, null, DamageEffects.Bleed);
    }

    void StopBleed()
    {
        isBleeding = false;
    }

    void FireDamage()
    {
        TakeDamage(0.2f, null, DamageEffects.Bleed);
    }

    void ResetItems()
    {
        LevelController.playerHasBow = false;
        LevelController.playerHasSword = false;
        LevelController.playerHasHelmet = false;
        LevelController.playerHasArmor = false;
        LevelController.playerHasFireBow = false;
        LevelController.playerHasAchillesArmor = false;
        LevelController.playerHasAchillesHelmet = false;
        LevelController.playerHasShield = false;
        LevelController.playerHasApolloBow = false;
        LevelController.playerHasHarpeSword = false;
        LevelController.playerHasPeleusSword = false;
        LevelController.playerBoostHp = 0;
        LevelController.playerBonusType = BonusTypes.None;
    }

    public void ShowEndPanel()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void CheckPickup()
    {
        helmetSprite.SetActive(LevelController.playerHasHelmet);
        helmetSprite.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;

        if (!hasArmor && LevelController.playerHasArmor) EquipArmor();
        if (!hasArmor && LevelController.playerHasAchillesArmor) EquipAchillesArmor();
        if (!achillesHelmetEffect && LevelController.playerHasAchillesHelmet) EquipAchillesHelmet();
        if (!hasLavaBoots && LevelController.playerHasLavaBoots) EquipLavaBoots();
        if (!hasPeleusSword && LevelController.playerHasPeleusSword) EquipPeleusSword();
        if (!hasHarpeSword && LevelController.playerHasHarpeSword) EquipHarpeSword();
        if (LevelController.playerHasSword) EquipDefaultSword();

        switch (LevelController.playerBonusType)
        {
            case BonusTypes.None: break;
            case BonusTypes.Moli: AddHealth(0.5f); LevelController.playerBonusType = BonusTypes.None; PlayAudio(bonusPickupClip); break;
            case BonusTypes.Baytulus: AddHealth(2f); LevelController.playerBonusType = BonusTypes.None; PlayAudio(bonusPickupClip); break;
            case BonusTypes.Cornucopia: AddHealth(1f); LevelController.playerBonusType = BonusTypes.None; PlayAudio(bonusPickupClip); break;
            case BonusTypes.Miasma: ApplyMiasmaEffect(); PlayAudio(bonusPickupClip); break;
            case BonusTypes.Ambrosia: ApplyAmbrosiaEffect(); PlayAudio(bonusPickupClip); break;
            case BonusTypes.Garnet: ApplyGarnetEffect(); LevelController.playerBonusType = BonusTypes.None; break;
            case BonusTypes.AriadneThread: PlayAudio(bonusPickupClip); break;
            case BonusTypes.FlaskOfIchor: maxHealth += 2; currentHealth += 1; LevelController.playerHp = currentHealth; LevelController.playerBoostHp = 2; ui.UpdateForMoreHp(); PlayAudio(bonusPickupClip); break;
            case BonusTypes.Panacea: maxHealth += 1; LevelController.playerBoostHp = 1; ui.UpdateForMoreHp(); PlayAudio(bonusPickupClip); break;
        }
        LevelController.playerBonusType = BonusTypes.None;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ExitPortal")
        {
            if (miasmaEffect) RemoveMiasmaEffect();
            if (ambrosiaEffect) RemoveAmbrosiaEffect();
        }
        if (collision.name == "Boss1") TakeDamage(0.5f, collision.gameObject);
    }

    private void ShowArmorTakeDamage()
    {
        helmetSprite.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.5f, 0.5f, 1f);
        achillesArmorSprite.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.5f, 0.5f, 1f);
        achillesHelmetSprite.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.5f, 0.5f, 1f);
        armorSprite.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.5f, 0.5f, 1f);
        Invoke("StopShowDamage", 0.1f);
    }

    private void StopShowDamage()
    {
        helmetSprite.GetComponent<SpriteRenderer>().color = Color.white;
        achillesArmorSprite.GetComponent<SpriteRenderer>().color = Color.white;
        achillesHelmetSprite.GetComponent<SpriteRenderer>().color = Color.white;
        armorSprite.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void ApplyAmbrosiaEffect()
    {
        ambrosiaEffect = true;
        ambrosiaEffectSprite.SetActive(true);
    }

    private void RemoveAmbrosiaEffect()
    {
        ambrosiaEffect = false;
        ambrosiaEffectSprite.SetActive(false);
        LevelController.playerBonusType = BonusTypes.None;
    }

    private void ApplyMiasmaEffect()
    {
        miasmaEffect = true;
        miasmaEffectSprite.SetActive(true);
    }

    private void RemoveMiasmaEffect()
    {
        miasmaEffect = false;
        miasmaEffectSprite.SetActive(false);
        LevelController.playerBonusType = BonusTypes.None;
    }

    private void ApplyGarnetEffect()
    {
        int rnd = Random.Range(0, 100);
        if (rnd < 50)
        {
            AddHealth(5);
            PlayAudio(bonusPickupClip);
        }
        else TakeDamage(10);
    }

    public void EquipArmor()
    {
        armorSprite.SetActive(true);
        goldenArmorEffect = true;
        hasArmor = true;
    }

    public void RemoveArmor()
    {
        armorSprite.SetActive(false);
        goldenArmorEffect = false;
        hasArmor = false;
    }

    public void EquipLavaBoots()
    {
        lavaBoots.SetActive(true);
        hasLavaBoots = true;
    }

    public void RemoveLavaBoots()
    {
        lavaBoots.SetActive(false);
        hasLavaBoots = false;
    }

    public void EquipAchillesArmor()
    {
        achillesArmorSprite.SetActive(true);
        achillesArmorEffect = true;
        hasArmor = true;
    }

    public void RemoveAchillesArmor()
    {
        achillesArmorSprite.SetActive(false);
        achillesArmorEffect = false;
        hasArmor = false;
    }

    public void EquipAchillesHelmet()
    {
        achillesHelmetSprite.SetActive(true);
        achillesHelmetEffect = true;
    }

    public void RemoveAchillesHelmet()
    {
        achillesHelmetSprite.SetActive(false);
        achillesHelmetEffect = false;
    }

    public void EquipDefaultSword()
    {
        defaultSwordSprite.enabled = true;
        LevelController.playerEquippedDefaultSword = true;
    }
    public void HideDefaultSword()
    {
        defaultSwordSprite.enabled = false;
        LevelController.playerEquippedDefaultSword = false;
    }

    public void RemoveDefaultSword()
    {
        defaultSwordSprite.enabled = false;
        LevelController.playerEquippedDefaultSword = false;
        LevelController.playerHasSword = false;
    }

    public void EquipPeleusSword()
    {
        peleusSwordSprite.enabled = true;
        LevelController.playerEquippedPeleusSword = true;
        hasPeleusSword = true;
    }

    public void HidePeleusSword()
    {
        peleusSwordSprite.enabled = false;
        LevelController.playerEquippedPeleusSword = false;
    }

    public void RemovePeleusSword()
    {
        peleusSwordSprite.enabled = false;
        LevelController.playerEquippedPeleusSword = false;
        LevelController.playerHasPeleusSword = false;
        hasPeleusSword = false;
    }

    public void EquipHarpeSword()
    {
        harpeSwordSprite.enabled = true;
        LevelController.playerEquippedHarpeSword = true;
        hasHarpeSword = true;
    }

    public void HideHarpeSword()
    {
        harpeSwordSprite.enabled = false;
        LevelController.playerEquippedHarpeSword = false;
    }

    public void RemoveHarpeSword()
    {
        harpeSwordSprite.enabled = false;
        LevelController.playerEquippedHarpeSword = false;
        LevelController.playerHasHarpeSword = false;
        hasHarpeSword = false;
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.Stop();
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void PlayBowSound()
    {
        audioSource.clip = bowShootClip;
        audioSource.Play();
    }
    public void PlaySwordSound()
    {
        audioSource.clip = swordAttackClip;
        audioSource.Play();
    }
}