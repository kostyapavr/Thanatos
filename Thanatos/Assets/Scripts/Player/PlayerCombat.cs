using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackDamage;
    public float attackRange;
    public float bonusAttackRange;

    public GameObject swordSprite;
    public Bow bow;
    public Sprite normalSprite;
    public Sprite swordPlayerSprite;
    private bool isHidden = false;

    private float attackDelay = 1.0f;
    private float time = 0.0f;

    //private ItemManager itemManager;

    private void Start()
    {
        //itemManager = GetComponent<ItemManager>();
        CheckPickup();
        LevelController.playerPickupItemEvent.AddListener(CheckPickup);
    }

    void Update()
    {
        if (time <= 0.0f && Input.GetKeyDown(KeyCode.Mouse0) && LevelController.playerHasSword && !bow.isCharging && !isHidden)
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        //swordSprite.SetActive(true);
        //bow.HideBow();
        animator.enabled = true;
        animator.Play("BasicSwordAnimation");
        Invoke("StopAnim", 0.25f);
    } 

    void StopAnim()
    {
        //swordSprite.SetActive(false);
        //bow.ShowBow();
        animator.enabled = false;
        GetComponent<SpriteRenderer>().sprite = swordPlayerSprite;
    }

    public void HideSword()
    {
        //swordSprite.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = normalSprite;
        isHidden = true;
    }

    public void ShowSword()
    {
        GetComponent<SpriteRenderer>().sprite = swordPlayerSprite;
        //swordSprite.SetActive(true);
        isHidden = false;
    }

    void CheckPickup()
    {
        if (LevelController.playerHasSword && LevelController.playerSelectedWeapon == 1)
        {
            ShowSword();
            LevelController.playerSelectedWeapon = 1;
        }
        else
        {
            HideSword();
            LevelController.playerSelectedWeapon = 0;
        }

        if (LevelController.playerHasHelmet && LevelController.playerHasSword)
        {
            attackRange = bonusAttackRange;
            attackDelay = 0.75f;
        }
    }
}

