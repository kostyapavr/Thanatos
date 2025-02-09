using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public AnimationClip clip;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange;
    public float bonusAttackRange;

    public GameObject swordSprite;
    public Bow bow;
    public Sprite normalSprite;
    public Sprite swordPlayerSprite;
    private bool isHidden = false;

    public IPickupableWeapon currentWeapon;

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
            /*if (currentWeapon == null)
            {
                Debug.Log("Current weapon is null!");
                return;
            }
            enemy.GetComponent<Enemy>().TakeDamage(currentWeapon.Damage);*/
            enemy.GetComponent<Enemy>().TakeDamage(1);
        }

        animator.enabled = true;
        //animator.SetBool("IsSwordAttack", true);
        Invoke("StopAnim", 0.25f);
    } 

    void StopAnim()
    {
        animator.enabled = false;
        //animator.SetBool("IsSwordAttack", false);
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

