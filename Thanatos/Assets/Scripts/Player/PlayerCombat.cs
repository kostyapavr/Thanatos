using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange;
    public float bonusAttackRange;
    public float bonusAttackDelay;
    public float peleusAttackRange;

    public Bow bow;
    public Sprite defaultSwordSprite;
    public Sprite peleusSwordSprite;
    private bool isHidden = false;

    public IPickupableWeapon currentWeapon;

    private float attackDelay = 1.0f;
    private float time = 0.0f;

    public SpriteRenderer defaultSpriteRenderer;
    public SpriteRenderer peleusSpriteRenderer;
    public Sprite[] defaultSwordSprites;
    public Sprite[] peleusSwordSprites;

    [HideInInspector]
    public bool anim = false;
    private int animCnt = 0;
    private int animMaxCnt = 3;

    private float animFrameInterval = 0.1f;
    private float animTime = 0.0f;

    [HideInInspector]
    public bool isAttacking = false;

    //private ItemManager itemManager;

    private void Start()
    {
        //itemManager = GetComponent<ItemManager>();
        CheckPickup();
        LevelController.playerPickupItemEvent.AddListener(CheckPickup);
    }

    void Update()
    {
        if (anim)
        {
            if (animTime <= 0.0f)
            {
                if (animCnt >= animMaxCnt)
                {
                    anim = false;
                    animCnt = 0;
                    animTime = animFrameInterval;
                    if (LevelController.playerHasSword) defaultSpriteRenderer.sprite = defaultSwordSprite;
                    if (LevelController.playerHasPeleusSword) peleusSpriteRenderer.sprite = peleusSwordSprite;
                    isAttacking = false;
                }

                if (LevelController.playerHasSword) defaultSpriteRenderer.sprite = defaultSwordSprites[animCnt];
                if (LevelController.playerHasPeleusSword) peleusSpriteRenderer.sprite = peleusSwordSprites[animCnt];

                animCnt++;
                animTime = animFrameInterval;
            }
            else animTime -= Time.deltaTime;
        }

        if (time <= 0.0f && Input.GetKeyDown(KeyCode.Mouse0) && (LevelController.playerEquippedDefaultSword || LevelController.playerEquippedPeleusSword) && !bow.isCharging && !isHidden)
        {
            Attack();
            time = attackDelay;
        }
        else
        {
            time -= Time.deltaTime;
        }
    }

    void Attack()
    {
        float range = attackRange;
        if (LevelController.playerEquippedPeleusSword) range = peleusAttackRange;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, range, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (LevelController.currentPlayerWeapon == null)
            {
                Debug.Log("Current weapon is null!");
                return;
            }

            DamageEffects dmgEffect = DamageEffects.Nothing;
            if (LevelController.playerEquippedPeleusSword) dmgEffect = DamageEffects.StopOneShot;
            enemy.GetComponent<Enemy>().TakeDamage(LevelController.currentPlayerWeapon.Damage, gameObject, dmgEffect);
        }

        PlayAnim();
        GetComponent<Player>().PlaySwordSound();
        isAttacking = true;
    } 

    void PlayAnim()
    {
        anim = true;
        animTime = animFrameInterval;
    }

    void CheckPickup()
    {
        if (LevelController.playerHasHelmet && LevelController.playerHasSword)
        {
            attackRange = bonusAttackRange;
            attackDelay = bonusAttackDelay;
        }
    }
}

