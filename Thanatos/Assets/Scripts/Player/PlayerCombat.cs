using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackDamage;
    public float attackRange;

    public GameObject swordSprite;
    public Bow bow;

    private float attackDelay = 1.0f;
    private float time = 0.0f;

    //private ItemManager itemManager;

    private void Start()
    {
        //itemManager = GetComponent<ItemManager>();
    }

    void Update()
    {
        if (time <= 0.0f && Input.GetKeyDown(KeyCode.Mouse1) && LevelController.playerHasSword)
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        swordSprite.SetActive(true);
        bow.canShoot = false;
        Invoke("StopAnim", 1);
    } 

    void StopAnim()
    {
        swordSprite.SetActive(false);
        bow.canShoot = true;
    }
}

